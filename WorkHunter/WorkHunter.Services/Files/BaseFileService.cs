using Common.DtoModels.Files;
using Common.Exceptions;
using Common.Models;
using Common.Models.Files;
using Common.Utils;
using Common.ViewModels.Files;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Abstractions.Files;
using WorkHunter.Data;

namespace WorkHunter.Services.Files
{
    public abstract class BaseFileService<TFile> : IBaseFileService<TFile> where TFile : BaseFile
    {
        protected readonly StorageOptions StorageOptions;

        protected readonly IWorkHunterDbContext WorkHunterDbContext;

        // TODO video-api/#51 : create common context for video and other files
        // protected readonly IFileDbContext Context;
        protected BaseFileService(
            IOptionsSnapshot<StorageOptions> storageOptions,
            IWorkHunterDbContext Context)
        {
            this.StorageOptions = storageOptions.Value;
            this.WorkHunterDbContext = Context;
        }

        // TODO video-api/#51: unify api via filters
        // protected override Func<int, Expression<Func<TEntity, bool>>> GetAllFilter 
        //     => (entityId) => (file) => file.EntityId == entityId;

        protected abstract Task CheckUserAccessToFile(TFile file);

        protected abstract Task CheckUserAccessToFileContainterEntity(Guid entityId);

        protected Action<TFile> UpdateFile;

        private string[] GetFilePathAsArray(Guid entityId)
            => new string[]
               {
                  StorageOptions.BasePath,
                  StorageOptions.VideoStorageFolder,
                  entityId.ToString()
               };

        public virtual async Task Delete(int fileId)
        {
            var file = await GetFileById(fileId);
            await CheckUserAccessToFile(file);

            File.Delete(file.Path);
            WorkHunterDbContext.Set<TFile>().Remove(file);
            await WorkHunterDbContext.SaveChangesAsync();
        }

        public virtual async Task<TFile> GetFileById(int fileId)
            => await WorkHunterDbContext.Set<TFile>()
                                        .FirstOrDefaultAsync(x => x.Id == fileId)
            ?? throw new EntityNotFoundException(fileId.ToString(), typeof(TFile).Name);

        public virtual async Task<IReadOnlyList<FileView>> GetAll(Guid containerEntityId)
        {
            await CheckUserAccessToFileContainterEntity(containerEntityId);
            return await WorkHunterDbContext.Set<TFile>()
                                            .ProjectToType<FileView>()
                                            .ToListAsync();
        }

        public virtual async Task<DownloadFile> GetContent(int fileId)
        {
            var file = await GetFileById(fileId);
            await CheckUserAccessToFile(file);

            await using var fileStream = File.OpenRead(file.Path);
            DownloadFile content = await FileUtils.ExportFileToStream(fileStream, file.Name);

            return content;
        }

        // TODO video-api/#51: made generic key for container entity, or generic container entity
        public virtual async Task<IReadOnlyList<TFile>> Upload(Guid entityId, IEnumerable<UploadFileDto> files, bool doValidation = true)
        {
            await CheckUserAccessToFileContainterEntity(entityId);
            // TODO video-api/#51 : System Limitation on users files
            // var filesCount = await WorkHunterDbContext.Set<TFile>().CountAsync(GetAllFilter(entityId));

            return await UploadFiles(files, GetFilePathAsArray(entityId), doValidation); // filesCount
        }

        // TODO video-api/#51: move in new common class
        private async Task<IReadOnlyList<TFile>> UploadFiles(IEnumerable<UploadFileDto> files, string[] strings, bool doValidation = true)
        {
            var directoryPath = Path.Combine(strings);

            if (doValidation)
                ValidateFiles(files, StorageOptions, directoryPath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var loaded = new List<string>();
            var created = new List<TFile>();

            try
            {
                foreach (var file in files)
                {
                    var filePath = Path.Combine(directoryPath, file.FileName);
                    var stream = file.Content.Value;
                    await using var fileStream = File.Create(filePath);
                    stream.Position = 0;
                    await stream.CopyToAsync(fileStream);
                    loaded.Add(filePath);

                    var entityFile = Activator.CreateInstance<TFile>();
                    entityFile.CreatedDate = DateTime.UtcNow;
                    entityFile.Path = filePath;
                    entityFile.Name = file.FileName;
                    UpdateFile.Invoke(entityFile);

                    created.Add(entityFile);
                    WorkHunterDbContext.Set<TFile>().Add(entityFile);
                }

                await WorkHunterDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                foreach (var uploadedFileName in loaded)
                    File.Delete(uploadedFileName);
            }

            return created;
        }

        private void ValidateFiles(IEnumerable<UploadFileDto> files, StorageOptions storageOptions, string filesDirectoryPath)
        {
            // TODO video-api/#51: check total files count per entity
            if (files.GroupBy(x => x.FileName).Any(x => x.Count() > 1))
                throw new BusinessErrorException("Загружаемые файлы сущности должны иметь разные названия!");

            if (files.Count() > storageOptions.MaxFileCount)
                throw new BusinessErrorException("Превышено количество загружаемых файлов!");

            foreach (var file in files)
            {
                if (file.Length > storageOptions.MaxFileSize)
                    throw new BusinessErrorException($"Превышен допустимый размер загружаемоего файла {file.FileName}! Файл не может первышать {(double)storageOptions.MaxFileSize / 1024 / 1024}");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!storageOptions.SupportedFormats.Contains(extension))
                    throw new BusinessErrorException("Загружаемый файл недопустимого формата!");

                CheckFileName(file.FileName);
            }
        }

        private static void CheckFileName(string fileName)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();

            if (fileName.IndexOfAny(invalidCharacters) >= 0)
                throw new BusinessErrorException($"Файл {fileName} содержит в наименовании недопустимые симфолы!");
        }
    }
}

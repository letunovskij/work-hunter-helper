using Common.DtoModels.Files;
using Common.Models;
using Common.Models.Files;
using Common.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Abstractions.Files
{
    public interface IBaseFileService<TFile> where TFile : BaseFile
    {
        /// <summary>
        /// Загрузить файлы в систему
        /// </summary>
        /// <param name="entityId">Идентификатор сущности-контейнера, к которой относятся загружаемые файлы</param>
        /// <param name="uploadFileModels">Список загружаемых файлов</param>
        /// <param name="doValidation">Требуется ли валидация на размер и формат</param>
        /// <returns></returns>
        Task<IReadOnlyList<TFile>> Upload(int entityId, IEnumerable<UploadFileDto> uploadFileModels, bool doValidation = true);

        Task<TFile> GetFileById(int fileId);

        /// <summary>
        /// Получить список файлов сущности
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<FileView>> GetAll(int entityId);

        /// <summary>
        /// Получить содержимое файла по идентификатору
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<DownloadFile> GetContent(int fileId);

        /// <summary>
        /// Удалить файл по идентификатору (hard удаление)
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task Delete(int fileId);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Services.Files;
using WorkHunter.Abstractions.Interviews;
using WorkHunter.Models.Entities.Interviews;
using Microsoft.Extensions.Options;
using Common.Models.Files;
using WorkHunter.Data;
using Abstractions.Users;
using Common.DtoModels.Files;

namespace WorkHunter.Services.Interviews
{
    public sealed class VideoInterviewFileService : BaseFileService<VideoInterviewFile>, IVideoInterviewFileService
    {
        private readonly IUserService userService;

        public VideoInterviewFileService(
            IOptionsSnapshot<StorageOptions> storageOptions, 
            IWorkHunterDbContext Context,
            IUserService userService) : base(storageOptions, Context)
        {
            this.userService = userService;
        }

        public override async Task<IReadOnlyList<VideoInterviewFile>> Upload(
            Guid entityId, IEnumerable<UploadFileDto> files, bool doValidation = true)
        {
            var currentUserId = (await userService.GetCurrent()).Id;

            base.UpdateFile = (fileEntity) =>
                {
                    fileEntity.CreatedById = currentUserId;
                    fileEntity.WResponseId = entityId;
                };
            return await base.Upload(entityId, files, doValidation);
        }

        protected override Task CheckUserAccessToFile(VideoInterviewFile file)
        {
            return Task.CompletedTask;
        }

        protected override Task CheckUserAccessToFileContainterEntity(Guid entityId)
        {
            return Task.CompletedTask;
        }
    }
}

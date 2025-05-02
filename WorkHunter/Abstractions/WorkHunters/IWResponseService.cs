using Common.Models;
using WorkHunter.Models.Dto.WorkHunters;
using WorkHunter.Models.Views.WorkHunters;

namespace WorkHunter.Abstractions.WorkHunters;

public interface IWResponseService
{
    Task<WResponseView> GetById(Guid guid);

    Task<IReadOnlyList<WResponseView>> GetAll();

    Task<WResponseView> Create(WResponseCreateDto dto);

    Task<WResponseView> Update(Guid guid, WResponseUpdateDto dto);

    Task Delete(Guid guid);

    Task Export();

    Task<DownloadFile?> ImportNewData(Stream stream);
}

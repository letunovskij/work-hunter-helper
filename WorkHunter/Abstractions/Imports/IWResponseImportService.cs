using ClosedXML.Excel;
using Common.Abstractions.Imports;
using Common.Models;

namespace WorkHunter.Abstractions.Imports
{
    public interface IWResponseImportService : IPageImportService
    {
        Task<DownloadFile?> ImportNewData(Stream stream);
        Task CheckOnExists();
    }
}

using ClosedXML.Excel;
using Common.Models;

namespace WorkHunter.Abstractions.WorkHunters
{
    public interface IWResponseImportService
    {
        Task Export();
        Task<DownloadFile?> ImportNewData(Stream stream);
        Task<Exception> AddToDb();
        Task CheckOnExists();
        void ImportDataToCollection(IXLWorksheet wresponsesWorksheet);
        void UpdatePageWithError(IXLWorksheet wresponsesWorksheet);
    }
}

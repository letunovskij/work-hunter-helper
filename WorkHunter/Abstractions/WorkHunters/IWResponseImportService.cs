using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Abstractions.WorkHunters
{
    public interface IWResponseImportService
    {
        Task<Exception> AddToDb();
        Task CheckOnExists();
        void ImportDataToCollection(IXLWorksheet wresponsesWorksheet);
        void UpdatePageWithError(IXLWorksheet wresponsesWorksheet);
    }
}

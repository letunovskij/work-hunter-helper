using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Abstractions.WorkHunters;

namespace WorkHunter.Services.WorkHunters
{
    public sealed class WResponseImportService : IWResponseImportService
    {
        public Task<Exception> AddToDb()
        {
            throw new NotImplementedException();
        }

        public Task CheckOnExists()
        {
            throw new NotImplementedException();
        }

        public void ImportDataToCollection(IXLWorksheet wresponsesWorksheet)
        {
            throw new NotImplementedException();
        }

        public void UpdatePageWithError(IXLWorksheet wresponsesWorksheet)
        {
            throw new NotImplementedException();
        }
    }
}

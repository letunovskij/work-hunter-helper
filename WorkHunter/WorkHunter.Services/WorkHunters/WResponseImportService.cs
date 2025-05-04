using ClosedXML.Excel;
using Common.Enums;
using Common.Extensions;
using Common.Models;
using Common.Utils;
using Microsoft.EntityFrameworkCore;
using WorkHunter.Abstractions.WorkHunters;
using WorkHunter.Data;
using WorkHunter.Models.Enums.Import;

namespace WorkHunter.Services.WorkHunters
{
    public sealed class WResponseImportService : IWResponseImportService
    {
        private readonly WorkHunterDbContext dbContext;
        public WResponseImportService(WorkHunterDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<DownloadFile?> ImportNewData(Stream stream)
        {
            var workbook = new XLWorkbook(stream);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await dbContext.WResponses.ExecuteDeleteAsync();
                await ImportData(workbook, ImportMode.RemoveAllThenAdd);

                await transaction.CommitAsync();
                return null;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                dbContext.ChangeTracker.Clear();

                return ExcelUtils.DownloadFile(workbook, $"wresponses-error-{DateTime.Now:s}");
            }
        }

        private async Task ImportData(XLWorkbook workbook, ImportMode mode)
        {
            var wresponsesWorksheet = workbook.Worksheets.First(x => x.Name == WresponsePageType.WresponsePage.GetDescription());

            this.ImportDataToCollection(wresponsesWorksheet);
            if (mode == ImportMode.Add)
                await this.CheckOnExists();

            var error = await this.AddToDb();
            if (error != null)
                this.UpdatePageWithError(wresponsesWorksheet);

            await dbContext.SaveChangesAsync();
        }

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


        public Task Export()
        {
            throw new NotImplementedException();
        }
    }
}

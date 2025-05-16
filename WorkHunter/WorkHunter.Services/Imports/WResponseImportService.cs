using ClosedXML.Excel;
using Common.Abstractions.Imports;
using Common.Enums;
using Common.Exceptions;
using Common.Extensions;
using Common.Models;
using Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkHunter.Abstractions.Imports;
using WorkHunter.Data;
using WorkHunter.Models.Enums.Import;
using WorkHunter.Models.Import;

namespace WorkHunter.Services.Imports
{
    public sealed class WResponseImportService : ImportService<WResponseImportModel>, IWResponseImportService
    {
        private readonly WorkHunterDbContext dbContext;

        private Dictionary<int, WResponseImportModel> ImportingKeyValuePairs { get; set; } = [];

        public WResponseImportService(WorkHunterDbContext dbContext, ILogger<WResponseImportService> logger) : base(logger) 
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
                var isError = await ImportData(workbook, ImportMode.RemoveAllThenAdd);

                if (isError)
                    throw new ImportException(); 

                await transaction.CommitAsync();

                return null;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                dbContext.ChangeTracker.Clear();

                return FileUtils.ExportWorkbookToStream(workbook, $"wresponses-error-{DateTime.Now:s}.xlsx");
            }
        }

        private async Task<bool> ImportData(XLWorkbook workbook, ImportMode mode)
        {
            var wresponsesWorksheet = workbook.Worksheets.First(x => x.Name == WresponsePageType.WresponsePage.GetDescription());

            ImportToCollection(wresponsesWorksheet);
            if (mode == ImportMode.Add)
                await CheckOnExists();

            var isError = await AddToEFCollection(wresponsesWorksheet);
            if (isError)
                UpdateImportingPageWithErrors(wresponsesWorksheet);

            await dbContext.SaveChangesAsync();

            return this.ImportExceptions.Count > 0;
        }

        public bool ImportToCollection(IXLWorksheet wresponsesWorksheet)
        {
            base.Clear();
            this.ImportingKeyValuePairs = base.ImportExcelPageDataToImportingCollection(wresponsesWorksheet);

            return base.ImportExceptions.Count > 0;
        }

        public override void UpdateImportingPageWithErrors(IXLWorksheet wresponsesWorksheet)
        {
            base.UpdateImportingPageWithErrors(wresponsesWorksheet);
            wresponsesWorksheet.TabColor = XLColor.MediumVioletRed;
        }

        public async Task<bool> AddToEFCollection(IXLWorksheet worksheet)
        {
            var users = await dbContext.Users.AsNoTracking().ToListAsync();

            foreach (var keyValuePair in this.ImportingKeyValuePairs)
            {
                var importingModel = keyValuePair.Value;
                var rowNumber = keyValuePair.Key;

                var user = users.Find(x => x.Id.Equals(importingModel.UserId));
                if (user == null && !string.IsNullOrEmpty(importingModel.UserId))
                    AddNotFoundError(rowNumber, WResponseImportModelConstants.UserId, importingModel.UserId, "Пользователей");

                var isErrorOnRow = this.ImportExceptions.Exists(x => x.RowNumber == rowNumber);

                if (!isErrorOnRow && IsModelValid(importingModel) && user != null) 
                    dbContext.WResponses.Add(new()
                    {
                        UserId = importingModel.UserId!,
                        Email = importingModel.Email,
                        VacancyUrl = importingModel.VacancyUrl!
                    });
            }

            return base.ImportExceptions.Count > 0;
        }

        private static bool IsModelValid(WResponseImportModel importingModel)
            => importingModel != null
            && !string.IsNullOrEmpty(importingModel.UserId) 
            && !string.IsNullOrEmpty(importingModel.VacancyUrl);

        // TODO проверить наличие в БД перед импортом
        public Task CheckOnExists()
        {
            throw new NotImplementedException();
        }
    }
}

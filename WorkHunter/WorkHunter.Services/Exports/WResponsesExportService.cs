using ClosedXML.Excel;
using Common.Abstractions.Exports;
using Common.Extensions;
using Common.Models;
using Common.Utils;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Abstractions.Exports;
using WorkHunter.Data;
using WorkHunter.Models.Enums.Import;

namespace WorkHunter.Services.Exports
{
    public sealed class WResponsesExportService : ExportService, IWResponsesExportService
    {
        private readonly IWorkHunterDbContext dbContext;

        public WResponsesExportService(IWorkHunterDbContext dbContext) : base() 
        {
            this.dbContext = dbContext;
        }

        protected override string TemplateFolder => "WResponses";

        protected override string TemplateName => "WResponsesTemplate.xlsx";

        public const string ExportName = "exported-wresponses";

        public async Task<DownloadFile> ExportToExcel()
        {
            var template = base.DownloadTemplate();
            using var workbook = new XLWorkbook(template.Data);

            await FillPage(workbook, WresponsePageType.UserPage);
            await FillPage(workbook, WresponsePageType.WresponsePage);

            return FileUtils.ExportWorkbookToStream(workbook, $"{ExportName}-{DateTime.Now:s}.xlsx");
        }

        private async Task FillPage(XLWorkbook workbook, WresponsePageType pageType)
        {
            var worksheet = workbook.Worksheets.First(x => x.Name.Trim() == pageType.GetDescription());
            var currentRowNumber = worksheet.LastRowUsed().RowNumber() + 1;

            switch (pageType)
            {
                case WresponsePageType.UserPage:
                    // TODO: move to userService, also use Mapster extensions for db access. It is more optimal, because less fields extracted from database.
                    var users = await dbContext.Users.AsNoTracking().ToListAsync();

                    foreach (var user in users)
                    {
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 1, user.Id);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 2, user.Name);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 3, user.UserName);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 4, user.Email);
                        currentRowNumber++;
                    }
                    break;

                case WresponsePageType.WresponsePage:
                    var wresponses = await dbContext.WResponses.AsNoTracking().ToListAsync();

                    foreach (var item in wresponses)
                    {
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 1, (int)item.Status);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 2, item.UserId);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 3, item.HhId);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 4, item.CreatedAt);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 5, item.IsAnswered);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 6, item.ViewedByMe);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 7, item.Contact);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 8, item.AnswerText);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 9, item.Email);
                        ExcelExportUtils.SetCell(worksheet, currentRowNumber, 10, item.VacancyUrl);
                        currentRowNumber++;
                    }
                    break;
            }
        }
    }
}

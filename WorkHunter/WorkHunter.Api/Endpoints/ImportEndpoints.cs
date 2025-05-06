using Microsoft.AspNetCore.Mvc;
using WorkHunter.Abstractions.Exports;
using WorkHunter.Abstractions.Imports;
using WorkHunter.Models.Constants;

namespace WorkHunter.Api.Endpoints;

internal static class ImportEndpoints
{
    private static string ExcelReportDefaultFileType = "application/octet-stream";

    internal static void MapImportEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("transfers")
                               .WithTags("Transfer")
                               .WithOpenApi();

        routeGroup.MapPost("WResponses/export", async (IWResponsesExportService service) => await service.Export())
            .RequireAuthorization(AppPolicies.Admin)
            .WithDescription("Экспортировать тестовые отклики");

        routeGroup.MapPost("WResponses/export-template", (IWResponsesExportService service)
            =>
        {
            var fileModel = service.DownloadTemplate();
            return Results.File(fileModel.Data, ExcelReportDefaultFileType, fileModel.Name);
        })
            .WithDescription("Скачать шаблон для загрузки существующих откликов в систему.");

        routeGroup.MapPost("WResponses/import", async ([FromForm] IFormFile formFile, IWResponseImportService service)
            => 
        {
            var fileModel = await service.ImportNewData(formFile.OpenReadStream());
            return fileModel == null ? Results.Ok() : Results.File(fileModel.Data, ExcelReportDefaultFileType, fileModel.Name);
        })
            .DisableAntiforgery()
            .RequireAuthorization(AppPolicies.Admin)
            .WithDescription("Импортировать тестовые отклики.");
    }
}

using Microsoft.AspNetCore.Mvc;
using WorkHunter.Abstractions.Interviews;
using WorkHunter.Api.Utils;
using WorkHunter.Models.Constants;

namespace WorkHunter.Api.Endpoints;

internal static class VideoInterviewEndpoints
{
    private static string VideoInterviewDefaultFileType = "application/octet-stream";

    internal static void MapVideoInterviewEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("responses")
                               .WithTags("VideoInterview")
                               .WithOpenApi();

        routeGroup.MapGet("{id:Guid}/files/{fileId}/get-content", async (Guid id, int fileId, IVideoInterviewFileService service)
            =>
        {
            var fileModel = await service.GetContent(fileId);
            return Results.File(fileModel.Data, VideoInterviewDefaultFileType, fileModel.Name);
        })
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Скачать видео интервью отклика.");

        // TODO: скачать превью файла

        // TODO: получить все видео-интервью по отклику

        routeGroup.MapPost("{id:Guid}/files", async (Guid id, [FromForm] IFormCollection formData, IVideoInterviewFileService service)
            =>
        {
            var fileStreams = FileUtils.GetFilesContent(formData.Files);
            await service.Upload(id, fileStreams);
        })
            .DisableAntiforgery()
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Загрузить видео интервью отклика.");

        routeGroup.MapPost("{id:guid}/file", async (Guid id, [FromForm] IFormFile file, IVideoInterviewFileService service)
            =>
        {
            var fileStreams = FileUtils.GetFilesContent(new FormFileCollection() { file });
            await service.Upload(id, fileStreams);
        })
            .DisableAntiforgery()
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Загрузить видео интервью отклика.");

        routeGroup.MapDelete("{id:guid}/file/{fileId}", async (Guid id, int fileId, IVideoInterviewFileService service) =>
        {
            await service.Delete(fileId);
        }).RequireAuthorization(AppPolicies.All)
          .WithDescription("Загрузить видео интервью отклика.");
    }
}

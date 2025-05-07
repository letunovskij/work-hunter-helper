using Microsoft.AspNetCore.Mvc;
using WorkHunter.Abstractions.WorkHunters;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Dto.WorkHunters;

namespace WorkHunter.Api.Endpoints;

internal static class WorkHuntersEndpoints
{
    internal static void MapWResponsesEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("responses")
                               .WithTags("Responses")
                               .WithOpenApi();

        routeGroup.MapGet(string.Empty, async (IWResponseService service)
            => await service.GetAll())
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить список всех откликов");

        routeGroup.MapGet("{guid:guid}", async (Guid guid, IWResponseService service)
            => await service.GetById(guid))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить отклик по guid");

        routeGroup.MapPut("{guid:guid}", async (Guid guid, [FromBody] WResponseUpdateDto dto, IWResponseService service)
            => await service.Update(guid, dto))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Обновить отклик по guid");

        routeGroup.MapPost(string.Empty, async ([FromBody] WResponseCreateDto dto, IWResponseService service)
            => await service.Create(dto))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Создать отклик");

        routeGroup.MapDelete("{guid:guid}", async (Guid guid, IWResponseService service)
            => await service.Delete(guid))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Удалить отклик");
    }
}

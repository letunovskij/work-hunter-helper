using WorkHunter.Abstractions.Enums;
using WorkHunter.Models.Enums;

namespace WorkHunter.Api.Endpoints;

internal static class EnumEndpoints
{
    internal static void MapEnumEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("enums")
                               .WithTags("Enums")
                               .WithOpenApi();

        routeGroup.MapGet("wresponses-statuses", (IEnumService service)
            => service.GetEnumValues<ResponseStatus>())
            .WithDescription("Получить описание статусов отклика");
    }
}

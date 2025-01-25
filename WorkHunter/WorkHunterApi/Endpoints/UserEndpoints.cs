using Abstractions.Users;
using Microsoft.AspNetCore.Mvc;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Dto.Users;

namespace WorkHunter.Api.Endpoints;

internal static class UserEndpoints
{
    internal static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("user")
                               .WithTags("User")
                               .WithOpenApi();

        routeGroup.MapGet("current", async (IUserService service) => await service.GetCurrent())
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить текущего пользователя");

        routeGroup.MapGet("token", async ([FromBody] LoginDto dto, IUserService service) => await service.Login(dto))
            .WithDescription("Получить токен доступа");
    }
}

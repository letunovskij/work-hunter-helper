using Abstractions.Users;
using Microsoft.AspNetCore.Mvc;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Dto.Users;

namespace WorkHunter.Api.Endpoints;

internal static class UserEndpoints
{
    internal static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("users")
                               .WithTags("User")
                               .WithOpenApi();

        routeGroup.MapGet("current", async (IUserService service) => await service.GetCurrent())
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить текущего пользователя")
            .RequireCors(options => options.AllowAnyOrigin());

        routeGroup.MapGet(string.Empty, async (IUserService service) => await service.GetAll())
            //.RequireAuthorization(AppPolicies.Admin)
            .WithDescription("Получить список всех пользователей")
            .RequireCors(options => options.AllowAnyOrigin()); 

        routeGroup.MapPost("token", async ([FromBody] LoginDto dto, IUserService service) => await service.Login(dto))
            .WithDescription("Получить токен доступа");

        routeGroup.MapPost("create", async ([FromBody] UserCreateDto dto, IUserService service) => await service.Create(dto))
            .WithDescription("Создать нового пользователя");
    }
}

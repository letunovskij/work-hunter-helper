using Microsoft.AspNetCore.Mvc;
using WorkHunter.Abstractions.Settings;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Dto.Settings;

namespace WorkHunter.Api.Endpoints;

internal static class AdminEndpoints
{
    internal static void MapAdminsEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("admins")
                               .WithTags("Admins")
                               .WithOpenApi();

        routeGroup.MapGet("banner", async (ISettingsService service) => await service.GetBannerSetting())
            .RequireAuthorization(AppPolicies.Admin)
            .WithDescription("Получить баннер");

        routeGroup.MapPost("banner", async ([FromBody] BannerSettingDto dto, ISettingsService service)
            => await service.UpdateBannerSetting(dto))
            .RequireAuthorization(AppPolicies.Admin)
            .WithDescription("Обновить настройки баннера");
    }
}
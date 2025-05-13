using Abstractions.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using WorkHunter.Models.Constants;

namespace WorkHunter.Api.Middleware;

public class UserAccessHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    private readonly IServiceScopeFactory defaultServiceScopeFactory;

    public UserAccessHandler(IServiceScopeFactory serviceScopeFactory) => this.defaultServiceScopeFactory = serviceScopeFactory;

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (await CheckUserIsBlocked(context))
            return;

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);

    }

    private async Task<bool> CheckUserIsBlocked(HttpContext context)
    {
        var userName = context.User?.Identity?.Name;

        if (string.IsNullOrEmpty(userName))
            return false;

        using var scope = defaultServiceScopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.GetByName(userName);

        if (user?.IsBlocked ?? false)
        {
            var admins = (await userService.GetInRoles(AppRoles.Admin)).Where(x => !x.IsBlocked);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"Пользователь {userName} заблокирован. Пожалуйста, обратитесь к администраторам {string.Join(", ", admins.Select(x => x.UserName))}.");

            return true;
        }

        return false;
    }
}

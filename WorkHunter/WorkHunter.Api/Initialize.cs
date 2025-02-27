using Microsoft.AspNetCore.Identity;
using WorkHunter.Data;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Entities.Users;

namespace WorkHunter.Api;

internal static class Initialize
{
    public static async Task SeedData(UserManager<User> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      IWorkHunterDbContext dbContext)
    {
        await SeedRoles(roleManager);
        await dbContext.SaveChangesAsync();

        await SeedUsers(userManager, dbContext);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedRoles(RoleManager<IdentityRole> manager)
    {
        await CreateRoleIfNotExists(manager, AppRoles.Admin);
        await CreateRoleIfNotExists(manager, AppRoles.User);
    }

    private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> manager, string role)
    {
        if (await manager.FindByIdAsync(role) == null)
            await manager.CreateAsync(new IdentityRole(role));
    }

    public static async Task SeedUsers(UserManager<User> manager, IWorkHunterDbContext dbContext)
    {
        await SeedUser(manager, new() { Name = "OLTest", UserName = "olluntest@mail.ru", Email = "olluntest@mail.ru" }, "WH!2025Admin", dbContext, AppRoles.Admin);
        await SeedUser(manager, new() { Name = "Admin@testuser.com", UserName = "Admin@testuser.com", Email = "Admin@testuser.com" }, "2025AdminWh!", dbContext, AppRoles.Admin);
        await SeedUser(manager, new() { Name = "User@testuser.com", UserName = "User@testuser.com", Email = "User@testuser.com" }, "2025UserWh!", dbContext, AppRoles.User);
    }

    private static async Task SeedUser(UserManager<User> manager, User user, string? password, IWorkHunterDbContext dbContext, params string[] roles)
    {
        var existedUser = await manager.FindByNameAsync(user.UserName);

        if (existedUser == null)
        {

            var result = !string.IsNullOrEmpty(password)
                         ? await manager.CreateAsync(user, password)
                         : await manager.CreateAsync(user);
            await dbContext.SaveChangesAsync();

            if (result.Succeeded)
            {
                existedUser = await manager.FindByNameAsync(user.UserName);
                await manager.AddToRolesAsync(existedUser!, roles);
            }
        }
    }
}

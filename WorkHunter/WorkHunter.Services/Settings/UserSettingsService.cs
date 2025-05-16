using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using WorkHunter.Abstractions.Settings;
using WorkHunter.Data;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Internal;

namespace WorkHunter.Services.Settings;

public sealed class UserSettingsService : BaseSettingService<string, UserSetting, UserSettingModel>, IUserSettingsService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    protected override Func<string, string, Expression<Func<UserSetting, bool>>> Get 
        => (userId, settingName) => entity => entity.UserId == userId && entity.Name == settingName;

    public UserSettingsService(IWorkHunterDbContext dbContext, IHttpContextAccessor httpContextAccessor) 
        : base(dbContext)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public override async Task<T> GetValue<T>(string settingName)
    {
        if (string.IsNullOrEmpty(httpContextAccessor?.HttpContext?.User?.Identity?.Name))
            throw new UnauthorizedAccessException("Текущий пользователь не авторизован!");

        var user = await workHunterDbContext.Users.SingleOrDefaultAsync(x => x.UserName == httpContextAccessor.HttpContext.User.Identity.Name);

        return await base.GetValue<T>(user!.Id, settingName);
    }

    public async Task UpdateSettings(string userId, IReadOnlyDictionary<string, JsonDocument> settings)
    {
        var user = await workHunterDbContext.Users
                                            .Include(x => x.Settings)
                                            .AsSplitQuery()
                                            .SingleOrDefaultAsync(x => x.Id == userId && !x.IsDeleted)
                                            ?? throw new EntityNotFoundException(userId, nameof(User));

        List<UserSettingModel> normalizeSettings = [.. settings.Select(x => new UserSettingModel { Name = x.Key, UserId = user.Id, Value = x.Value, IsDeleted = false })];

        user.Settings ??= new List<UserSetting>(); // TODO: check when EF Core returns null collection instead of empties

        await base.UpdateSettings(normalizeSettings, user.Settings);

        await workHunterDbContext.SaveChangesAsync();
    }
}

using Common.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WorkHunter.Abstractions.Settings;
using WorkHunter.Data;
using WorkHunter.Models.Constants.Settings;
using WorkHunter.Models.Dto.Settings;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Views.Settings;

namespace WorkHunter.Services.Settings;

public sealed class SettingsService : ISettingsService
{
    private static readonly JsonSerializerOptions serializationOptions = new(JsonSerializerDefaults.Web);

    private readonly IWorkHunterDbContext workHunterDbContext;

    public SettingsService(IWorkHunterDbContext workHunterDbContext)
    {
        this.workHunterDbContext = workHunterDbContext;
    }

    public async Task<T?> GetValue<T>(string settingName)
    {
        var setting = await workHunterDbContext.Set<SystemSetting>()
                                               .AsNoTracking()
                                               .SingleOrDefaultAsync(x => !x.IsDeleted && x.Name == settingName);

        if (setting == null)
            return default;

        return setting.Value.Deserialize<T>(serializationOptions)
            ?? throw new BusinessErrorException($"Не удалось привести настройку name={settingName} к типу {typeof(T).Name}!");
    }

    public async Task<BannerSettingView> GetBannerSetting()
    {
        var banner = await GetValue<BannerSettingDto>(SettingConstants.Banner);
        return banner.Adapt<BannerSettingView>();
    }

    public async Task<BannerSettingView> UpdateBannerSetting(BannerSettingDto bannerDto)
    {
        var setting = await workHunterDbContext.Set<SystemSetting>()
                                               .AsNoTracking()
                                               .SingleOrDefaultAsync(x => !x.IsDeleted && x.Name == SettingConstants.Banner);

        if (setting != null)
        {
            var bannerSetting = await GetValue<BannerSettingDto>(SettingConstants.Banner);
            bannerDto.Adapt(bannerSetting);
            setting.Value = JsonSerializer.SerializeToDocument(bannerDto, serializationOptions) ?? throw new BusinessErrorException($"Не удалось привести настройку name={SettingConstants.Banner} к типу {typeof(BannerSettingDto).Name}!");
        }
        else
        {
            var banner = JsonSerializer.SerializeToDocument(bannerDto, serializationOptions) ?? throw new BusinessErrorException($"Не удалось привести настройку name={SettingConstants.Banner} к типу {typeof(BannerSettingDto).Name}!");
            workHunterDbContext.SystemSettings.Add(new SystemSetting()
            {
                Name = SettingConstants.Banner,
                Value = banner
            });
        }

        await workHunterDbContext.SaveChangesAsync();
        return await GetBannerSetting();
    }
}

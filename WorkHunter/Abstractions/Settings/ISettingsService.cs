using WorkHunter.Models.Dto.Settings;
using WorkHunter.Models.Views.Settings;

namespace WorkHunter.Abstractions.Settings;

public interface ISettingsService
{
    Task<T?> GetValue<T>(string settingName);

    Task<BannerSettingView> GetBannerSetting();

    Task<BannerSettingView> UpdateBannerSetting(BannerSettingDto bannerDto);
}

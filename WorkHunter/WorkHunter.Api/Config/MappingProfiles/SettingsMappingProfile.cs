using Mapster;
using WorkHunter.Models.Dto.Settings;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Internal;
using WorkHunter.Models.Views.Settings;

namespace WorkHunter.Api.Config.MappingProfiles;

public sealed class SettingsMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserSettingModel, UserSetting>();

        config.NewConfig<BannerSettingDto, BannerSettingView>();
    }
}

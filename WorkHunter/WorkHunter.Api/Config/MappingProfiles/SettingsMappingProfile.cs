using Mapster;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Internal;

namespace WorkHunter.Api.Config.MappingProfiles;

public sealed class SettingsMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserSettingModel, UserSetting>();
    }
}

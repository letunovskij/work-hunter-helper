using WorkHunter.Models.Entities.Settings;

namespace WorkHunter.Models.Internal;

public sealed class UserSettingModel : BaseSetting
{
    public required string UserId { get; set; }
}

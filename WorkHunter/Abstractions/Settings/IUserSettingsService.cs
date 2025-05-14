using System.Text.Json;

namespace WorkHunter.Abstractions.Settings
{
    public interface IUserSettingsService : IBaseSettingService<string>
    {
        Task UpdateSettings(string userId, IReadOnlyDictionary<string, JsonDocument> settings);
    }
}

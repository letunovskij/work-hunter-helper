namespace WorkHunter.Abstractions.Settings;

public interface IBaseSettingService<TForeignKey>
{
    Task<T> GetValue<T>(string settingName);

    Task<T> GetValue<T>(TForeignKey id, string settingName);

    Task<T?> GetOptionalValue<T>(TForeignKey id, string settingName);
}

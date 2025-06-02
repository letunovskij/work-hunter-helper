using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using WorkHunter.Abstractions.Settings;
using WorkHunter.Data;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Services.Utils;

namespace WorkHunter.Services.Settings;

public abstract class BaseSettingService<TForeignKey, TEntity, TDtoEntity> : IBaseSettingService<TForeignKey>
    where TEntity : BaseSetting
    where TDtoEntity : BaseSetting
{
    protected readonly IWorkHunterDbContext workHunterDbContext;

    private static readonly JsonSerializerOptions serializationOptions = new(JsonSerializerDefaults.Web);

    public abstract Task<T> GetValue<T>(string settingName);

    protected abstract Func<TForeignKey, string, Expression<Func<TEntity, bool>>> Get { get; }
    
    public BaseSettingService(IWorkHunterDbContext dbContext)
    {
        this.workHunterDbContext = dbContext;
    }

    public virtual async Task<T> GetValue<T>(TForeignKey id, string settingName)
    {
        var setting = await workHunterDbContext.Set<TEntity>()
                                               .AsNoTracking()
                                               .SingleOrDefaultAsync(Get(id, settingName))
                                               ?? throw new BusinessErrorException($"настройка name={settingName} для сущности id={id} не найдена!");

        return setting.Value.Deserialize<T>(serializationOptions)
            ?? throw new BusinessErrorException($"Не удалось привести настройку name={settingName} для сущности id={id} к типу {typeof(T).Name}!");
    }

    public virtual async Task<T?> GetOptionalValue<T>(TForeignKey id, string settingName)
    {
        var setting = await workHunterDbContext.Set<TEntity>()
                                               .AsNoTracking()
                                               .SingleOrDefaultAsync(Get(id, settingName))
                                               ?? throw new BusinessErrorException($"настройка name={settingName} для сущности id={id} не найдена!");

        return (setting == null) ? default : setting.Value.Deserialize<T>(serializationOptions)
            ?? throw new BusinessErrorException($"Не удалось привести настройку name={settingName} для сущности id={id} к типу {typeof(T).Name}!");
    }

    protected virtual async Task UpdateSettings(ICollection<TDtoEntity> dtoSettings, ICollection<TEntity> dbSettings)
    {
        CollectionUtils.UpdateDestinationFromSource(dbSettings, dtoSettings, dst => dst.Name, src => src.Name, StringComparer.Ordinal, removeFromSourceNotFounded: false);

        await workHunterDbContext.SaveChangesAsync();
    }
}

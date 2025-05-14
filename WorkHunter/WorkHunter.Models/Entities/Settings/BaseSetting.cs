using System.Text.Json;

namespace WorkHunter.Models.Entities.Settings;

public abstract class BaseSetting
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public required string Name { get; set; }

    public required JsonDocument Value { get; set; }
}

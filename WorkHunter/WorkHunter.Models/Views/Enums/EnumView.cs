namespace WorkHunter.Models.Views.Enums;

public sealed class EnumView<T> where T : Enum
{
    public required T Value { get; set; }

    public string? Name { get; set; }
}

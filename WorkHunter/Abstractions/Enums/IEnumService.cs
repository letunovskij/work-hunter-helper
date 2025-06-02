using WorkHunter.Models.Views.Enums;

namespace WorkHunter.Abstractions.Enums;

public interface IEnumService
{
    IReadOnlyList<EnumView<TEnum>> GetEnumValues<TEnum>() where TEnum : Enum; 
}

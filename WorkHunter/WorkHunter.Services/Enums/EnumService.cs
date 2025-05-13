using Common.Extensions;
using WorkHunter.Abstractions.Enums;
using WorkHunter.Models.Views.Enums;

namespace WorkHunter.Services.Enums;

public sealed class EnumService : IEnumService
{
    public IReadOnlyList<EnumView<TEnum>> GetEnumValues<TEnum>() where TEnum : Enum
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

        return values.Select(x => new EnumView<TEnum>()
        {
            Name = x.GetDescription(),
            Value = x
        }).ToList();
    }
}

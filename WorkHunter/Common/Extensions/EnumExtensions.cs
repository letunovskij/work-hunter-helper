using System.ComponentModel;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDescription(this Enum value) 
            => value.GetType()
                    ?.GetMember(value.ToString())
                    ?.FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
    }
}

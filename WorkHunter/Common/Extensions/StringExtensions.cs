using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string? input1, string? input2)
            => string.IsNullOrEmpty(input1) ? string.IsNullOrEmpty(input2)
                                            : string.Equals(input1.Trim(), input2?.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}

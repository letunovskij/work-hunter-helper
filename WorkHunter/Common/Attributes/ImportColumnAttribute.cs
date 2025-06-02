using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImportColumnAttribute : ImportHeaderAttribute
    {
        public bool IsRequired { get; set; }

        public DigitType DigitType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkHunterUtils.Models.Models
{
    public class CurrencyHistory : BaseEntity, IDisposable
    {
        public DateTime RequestDate { get; set; }

        public required JsonDocument CurrenciesValues { get; set; }

        public void Dispose()
        {
            CurrenciesValues.Dispose();
        }
    }
}

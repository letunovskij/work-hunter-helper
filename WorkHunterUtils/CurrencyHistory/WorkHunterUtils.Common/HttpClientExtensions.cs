using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunterUtils.Common
{
    public static class HttpClientExtensions
    {
        public static bool IsInvalidHost(this HttpClient client) => client.BaseAddress == null;
    }
}

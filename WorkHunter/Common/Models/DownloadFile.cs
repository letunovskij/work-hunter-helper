using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public sealed class DownloadFile
    {
        public required string Name { get; set; }

        public required Stream Data { get; set; }
    }
}

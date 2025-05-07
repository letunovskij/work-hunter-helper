using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Files
{
    public class BaseFile
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Path { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}

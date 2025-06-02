using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Imports
{
    public sealed class ImportExceptionModel
    {
        public int? RowNumber { get; set; }

        public int? ColumnNumber { get; set; }

        public string? ColumnLetter { get; set; }

        public required string Message { get; set; }

        public Exception? Exception { get; set; }
    }
}

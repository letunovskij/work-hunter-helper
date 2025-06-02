using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public sealed class ImportException : Exception
    {
        public ImportException() { }

        public ImportException(string message) : base(message) { }

        public ImportException(string message, Exception innerException) : base(message, innerException) { }
    }
}

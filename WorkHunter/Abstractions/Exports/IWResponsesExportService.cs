using Common.Abstractions.Exports;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Abstractions.Exports
{
    public interface IWResponsesExportService : IExportService
    {
        Task Export();
    }
}

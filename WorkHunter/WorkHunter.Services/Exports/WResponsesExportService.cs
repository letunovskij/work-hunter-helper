using Common.Abstractions.Exports;
using Common.Models;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Abstractions.Exports;

namespace WorkHunter.Services.Exports
{
    public sealed class WResponsesExportService : ExportService, IWResponsesExportService
    {
        public WResponsesExportService() : base() { }

        protected override string TemplateFolder => "WResponses";

        protected override string TemplateName => "WResponsesTemplate.xlsx";

        public Task Export()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Models.Constants.Import;

namespace WorkHunter.Models.Enums.Import
{
    public enum WresponsePageType
    {
        [Description(WResponsesExportConstants.UserPage)]
        UserPage = 1,

        [Description(WResponsesExportConstants.WresponsePage)]
        WresponsePage = 2
    }
}

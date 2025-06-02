using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstractions.Exports
{
    public interface IExportService
    {
        DownloadFile DownloadTemplate();

        DownloadFile DownloadTemplateByName(string templateFolder, string templateName);
    }
}

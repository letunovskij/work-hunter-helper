using ClosedXML.Excel;
using Common.Models;
using System.Reflection;

namespace Common.Utils
{
    public static class ExcelUtils
    {
        public static DownloadFile DownloadFile(XLWorkbook xLWorkbook, string fileName)
        {
            var memoryStream = new MemoryStream();
            xLWorkbook.SaveAs(memoryStream);
            memoryStream.Position = 0;

            return new DownloadFile { Name = fileName, Data = memoryStream };
        }

        public static DownloadFile ReadTemplateFile(string templateFolder, string templateName, string templateExtension)
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(appPath, "Templates", $"{templateFolder}", $"{templateName}{templateExtension}");
            var data = File.OpenRead(filePath);
            return new DownloadFile { Name = $"{templateName}{templateExtension}", Data = data };
        }
    }
}

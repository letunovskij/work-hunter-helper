using ClosedXML.Excel;
using Common.Exceptions;
using Common.Models;
using System.Reflection;

namespace Common.Utils
{
    public static class FileUtils
    {
        public static DownloadFile ExportWorkbookToStream(XLWorkbook xLWorkbook, string fileName)
        {
            var memoryStream = new MemoryStream();
            xLWorkbook.SaveAs(memoryStream);
            memoryStream.Position = 0;

            return new DownloadFile { Name = fileName, Data = memoryStream };
        }

        public static DownloadFile ReadTemplateFile(string? templateFolder, string? templateName)
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(appPath))
                throw new BusinessErrorException("Не удалось определить расположение исполняемого приложения.");

            var filePath = Path.Combine(appPath, "Templates", $"{templateFolder}", $"{templateName}");
            var data = File.OpenRead(filePath);
            return new DownloadFile { Name = $"{templateName}", Data = data };
        }

    }
}

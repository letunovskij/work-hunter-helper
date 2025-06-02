using Common.Exceptions;
using Common.Models;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstractions.Exports
{
    public abstract class ExportService : IExportService
    {
        protected virtual string? TemplateFolder { get; init; }
        protected virtual string? TemplateName { get; init; }

        public virtual DownloadFile DownloadTemplate()
        {
            if (string.IsNullOrEmpty(this.TemplateFolder) || string.IsNullOrEmpty(this.TemplateName))
                throw new BusinessErrorException($"Шаблон импорта {this.GetType().Name} не сконфигурирован.");

            return FileUtils.ReadTemplateFile(this.TemplateFolder, this.TemplateName);
        }

        public virtual DownloadFile DownloadTemplateByName(string templateFolder, string templateName)
            => FileUtils.ReadTemplateFile(templateFolder, templateName);
    }
}

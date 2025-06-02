using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstractions.Imports
{
    public interface IPageImportService : IImportService
    {
        /// <summary>
        /// импортировать лист в коллекцию сервиса
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns>флаг наличия ошибок при импорте</returns>
        bool ImportToCollection(IXLWorksheet worksheet);

        /// <summary>
        /// добавить коллекцию сервиса в EF. Ошибки, если они есть, добавить в коллецию исключений сервиса.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns>флаг наличия ошибок при импорте</returns>
        Task<bool> AddToEFCollection(IXLWorksheet worksheet);

        /// <summary>
        /// обновить импортируемый лист найденными ошибками
        /// </summary>
        /// <param name="worksheet"></param>
        void UpdateImportingPageWithErrors(IXLWorksheet worksheet);
    }
}

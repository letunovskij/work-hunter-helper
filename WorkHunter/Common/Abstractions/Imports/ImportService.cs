using ClosedXML.Excel;
using Common.Abstractions.Imports;
using Common.Attributes;
using Common.Constants.Imports;
using Common.Models.Imports;
using Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Abstractions.Imports
{
    public abstract class ImportService<TImportModel> : IImportService
        where TImportModel : IImportModel
    {
        protected readonly int PageHeaderNumber = ImportConstants.DefaultHeaderRow;
        protected int NumberColumnForErrors;
        protected Dictionary<string, (int number, string letter)> ColumnDictionary = [];
        protected List<ImportExceptionModel> ImportExceptions = [];
        private readonly CultureInfo russianCulture = new("ru-RU");
        private readonly ILogger logger;

        protected ImportService(ILogger logger)
        {
            this.logger = logger;
        }

        protected void Clear()
        {
            this.ImportExceptions = [];
            this.ColumnDictionary = [];
        }

        protected Dictionary<int, TImportModel> ImportExcelPageDataToImportingCollection(IXLWorksheet worksheet)
        {
            var modelProperties = typeof(TImportModel).GetProperties()
                                                      .Where(x => Attribute.IsDefined(x, typeof(ImportColumnAttribute)));

            var headerColumns = new Dictionary<int, string>();
            var headerRow = worksheet.Row(this.PageHeaderNumber);
            this.NumberColumnForErrors = headerRow.CellsUsed().Count() + 1;

            AddColumnHeadersFromHeaderRow(modelProperties, headerColumns, headerRow);
            return ImportRowsToImportingCollection(worksheet, modelProperties, headerColumns);
        }

        private Dictionary<int, TImportModel> ImportRowsToImportingCollection(
            IXLWorksheet worksheet, IEnumerable<PropertyInfo> modelImportedProps, Dictionary<int, string> headerColumns)
        {
            var importingCollection = new Dictionary<int, TImportModel>();

            foreach (var row in worksheet.RowsUsed().Skip(this.PageHeaderNumber))
            {
                if (row.IsEmpty())
                    continue;
                var rowNumber = row.RowNumber();

                var range = worksheet.Range(worksheet.Cell(rowNumber, 1).Address, worksheet.Cell(rowNumber, this.NumberColumnForErrors - 1).Address);

                var importingModel = Activator.CreateInstance<TImportModel>();
                foreach (var cell in range.Cells())
                {
                    var importedModelProperty = modelImportedProps.FirstOrDefault(
                        x => string.Equals(
                                x.GetCustomAttribute<ImportColumnAttribute>()?.Name,
                                headerColumns[cell.Address.ColumnNumber],
                                StringComparison.OrdinalIgnoreCase));

                    if (importedModelProperty == null)
                        continue;

                    var importedColumnAttribute = importedModelProperty.GetCustomAttribute<ImportColumnAttribute>();
                    if (importedColumnAttribute == null)
                        continue;

                    try
                    {
                        var cellValue = cell.Value.ToString();

                        if (importedColumnAttribute.IsRequired && string.IsNullOrEmpty(cellValue))
                            this.ImportExceptions.Add(new()
                            {
                                Message = $"Ячейка {cell.Address.RowNumber}{cell.Address.ColumnLetter} обязательна для заполнения!",
                                RowNumber = cell.Address.RowNumber,
                                ColumnLetter = cell.Address.ColumnLetter,
                                ColumnNumber = cell.Address.ColumnNumber
                            });

                        object? convertingCellValue;

                        if (Nullable.GetUnderlyingType(importedModelProperty.PropertyType) is not null && string.IsNullOrEmpty(cellValue))
                            convertingCellValue = null;
                        else
                        {
                            // TODO: decimal импортируются в разных культурах по-разному из-за разных разделителей
                            if (importedModelProperty.PropertyType.Name == typeof(decimal[]).Name)
                            {
                                if (string.IsNullOrEmpty(cellValue))
                                    convertingCellValue = null;
                                else
                                {
                                    var source = cellValue.Split(';');
                                    convertingCellValue = source.Select(x => decimal.Parse(x, NumberStyles.Any)).ToArray();
                                }
                            }
                            else if (importedModelProperty.PropertyType.Name == typeof(decimal).Name
                                     || Nullable.GetUnderlyingType(importedModelProperty.PropertyType)?.Name == typeof(decimal).Name)
                                convertingCellValue = decimal.Parse(cellValue, NumberStyles.Any);
                            // TODO даты из документов и веб страниц могут быть разного формата
                            //else if (importedModelProperty.PropertyType.Name == typeof(DateOnly).Name
                            //         || Nullable.GetUnderlyingType(importedModelProperty.PropertyType)?.Name == typeof(DateOnly).Name)
                            //{} 
                            else
                                convertingCellValue = Convert.ChangeType(cellValue,
                                    Nullable.GetUnderlyingType(importedModelProperty.PropertyType) ?? importedModelProperty.PropertyType, CultureInfo.InvariantCulture);

                            if (importedColumnAttribute.DigitType != 0)
                            {
                                var cellValueAsDigital = decimal.Parse(cellValue, NumberStyles.Any);

                                // TODO числовые параметры вакансий зависят от типа поля
                                switch (importedColumnAttribute.DigitType)
                                {
                                    //case DigitType.Positive:
                                    //    if (DigitUtils.CheckIsPercent(cellValueAsDigital)) AddRangeException(cell);
                                }
                            }
                        }

                        // TODO в документах могут быть escape последовательности, вертикальные табы и прочие символы
                        //if (convertingCellValue is string)
                        //    convertingCellValue = StringUtils.RemoveSpecSymbols(convertingCellValue.ToString());

                        importedModelProperty.SetValue(importingModel, convertingCellValue);
                    }
                    catch (Exception ex)
                    {
                        ImportExceptions.Add(new()
                        {
                            Message = $"Некорректный тип данных в ячейке {cell.Address.RowNumber}{cell.Address.ColumnLetter}",
                            RowNumber = cell.Address.RowNumber,
                            ColumnLetter = cell.Address.ColumnLetter,
                            ColumnNumber = cell.Address.ColumnNumber,
                            Exception = ex
                        });
                    }
                }

                // копии строк игнорируются
                if (importingCollection.ContainsValue(importingModel))
                    continue;

                // строки, совпадающие по уникальному ключу, подсвечиваются как ошибки.
                // в тексте ошибки указывать номер оригинала строки.
                if (importingModel is IImportUniqueModel<TImportModel> uniqueModel)
                {
                    (bool contains, int? keyOfOriginalRow) = DictionaryUtils.ContainsByKey(importingCollection, uniqueModel);

                    if (contains && keyOfOriginalRow != null)
                        ImportExceptions.Add(new()
                        {
                            Message = $"Копия ключа в строке {keyOfOriginalRow.Value}",
                            RowNumber = rowNumber
                        });

                    if (!contains)
                    {
                        importingCollection.TryAdd(rowNumber, importingModel);
                        continue;
                    }
                }
                
                importingCollection.TryAdd(rowNumber, importingModel);
            }

            return importingCollection;
        }

        private void AddColumnHeadersFromHeaderRow(IEnumerable<PropertyInfo> modelProperties, Dictionary<int, string> headerColumns, IXLRow headerRow)
        {
            foreach (var headerCell in headerRow.CellsUsed())
            {
                try
                {
                    var headerCellText = headerCell.Value.ToString().Trim();
                    headerColumns.Add(headerCell.Address.ColumnNumber, headerCellText);
                    this.ColumnDictionary.Add(headerCellText, (headerCell.Address.ColumnNumber, headerCell.Address.ColumnLetter));
                } 
                catch (Exception ex)
                {
                    this.ImportExceptions.Add(new()
                    {
                        Message = $"Дубликат колонки в ячейке {headerCell.Address.RowNumber}{headerCell.Address.ColumnLetter}!",
                        RowNumber = headerCell.Address.RowNumber,
                        ColumnLetter = headerCell.Address.ColumnLetter,
                        ColumnNumber = headerCell.Address.ColumnNumber,
                        Exception = ex
                    });
                }
            }

            var headersColumnsNames = headerColumns.Values.ToList();
            var modelColumnsNames = modelProperties.Select(x => (x.GetCustomAttribute(typeof(ImportColumnAttribute)) as ImportColumnAttribute)?.Name);

            foreach (var modelColumnName in modelColumnsNames.Where(n => !string.IsNullOrEmpty(n))) 
            {
                if (!headersColumnsNames.Contains(modelColumnName))
                    this.ImportExceptions.Add(new()
                    {
                        Message = $"Пропущена колонка {modelColumnName} в файле импорта!",
                        RowNumber = this.PageHeaderNumber,
                    });
            }
        }

        public void AddNotFoundError<TValue>(int rowNumber, string columnNumber, TValue value, string tableName)
        {
            var (number, letter) = this.ColumnDictionary[columnNumber];
            this.ImportExceptions.Add(new()
            {
                Message = $"В таблице {tableName} не найдено значение {value} ячейки {rowNumber}{letter}!",
                RowNumber = rowNumber,
                ColumnNumber = number,
                ColumnLetter = letter
            });
        }

        public virtual void UpdateImportingPageWithErrors(IXLWorksheet worksheet)
        {
            if (!ImportExceptions.Any())
                return;

            worksheet.Cell(1, this.NumberColumnForErrors).SetValue("Ошибки");
            worksheet.Column(this.NumberColumnForErrors).Style.Font.FontColor = XLColor.RadicalRed;

            StringBuilder commonErrors = new();

            var headersExceptions = ImportExceptions.Where(x => x.RowNumber == this.PageHeaderNumber || x.RowNumber == null);
            if (headersExceptions.Any())
            {
                foreach (var exception in headersExceptions)
                {
                    commonErrors.AppendLine(exception.Message).Append(' ');

                    if (exception.RowNumber != null && exception.ColumnNumber != null)
                        worksheet.Cell(exception.RowNumber.Value, exception.ColumnNumber.Value).Style.Fill.BackgroundColor = XLColor.RadicalRed;
                }

                worksheet.Cell(this.PageHeaderNumber, this.NumberColumnForErrors).SetValue(commonErrors.ToString());
            }

            foreach (var row in worksheet.RowsUsed().Skip(this.PageHeaderNumber))
            {
                StringBuilder rowErrors = new();
                var rowExceptions = ImportExceptions.Where(x => x.RowNumber == row.RowNumber());

                if (rowExceptions.Any())
                {
                    foreach (var exception in rowExceptions)
                    {
                        if (exception.RowNumber != null && exception.ColumnNumber != null)
                            worksheet.Cell(exception.RowNumber.Value, exception.ColumnNumber.Value).Style.Fill.BackgroundColor = XLColor.RadicalRed;

                        if (exception.RowNumber != null && exception.ColumnNumber == null)
                            worksheet.Row(exception.RowNumber.Value).Style.Fill.BackgroundColor = XLColor.MediumRedViolet;

                        rowErrors.Append(exception.Message).Append(' ');
                    }
                    worksheet.Cell(row.RowNumber(), this.NumberColumnForErrors).SetValue(rowErrors.ToString());
                }
            }

            worksheet.Columns().AdjustToContents();

            LogErrors();
        }

        private void LogErrors()
        {
            foreach (var exception in this.ImportExceptions)
            {
                logger.LogError(exception.Exception, """
                    {@Model} was not imported.
                    Error: {@Message}
                    """, typeof(TImportModel).Name, exception.Message);
            }
        }
    }
}

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class ExcelExportUtils
    {
        private const string FontName = "Arial";
        private const int FontSize = 11;
        private static readonly XLColor FontHeaderColor = XLColor.White;

        private static void SetCellBase(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            worksheet.Cell(rowNumber, columnNumber).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(rowNumber, columnNumber).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(rowNumber, columnNumber).Style.Alignment.WrapText = true;
            worksheet.Cell(rowNumber, columnNumber).Value = value;
        }

        public static void SetRange(IXLWorksheet worksheet, int rowNumber, int columnStart, int columnEnd, XLCellValue value)
        {
            var range = worksheet.Range(worksheet.Cell(rowNumber, columnStart), worksheet.Cell(rowNumber, columnEnd)).Merge();
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        public static void SetCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            SetCellBase(worksheet, rowNumber, columnNumber, value);
        }

        public static void SetPercentageCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            worksheet.Cell(rowNumber, columnNumber).Style.NumberFormat.Format = "0.00%";
            SetCellBase(worksheet, rowNumber, columnNumber, value);
        }

        public static void SetRoundDecimalCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            worksheet.Cell(rowNumber, columnNumber).Style.NumberFormat.Format = "#,##0.00";
            SetCellBase(worksheet, rowNumber, columnNumber, value);
        }

        public static void SetHeaderCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            worksheet.Cell(rowNumber, columnNumber).Style.Font.SetBold();
            SetCellBase(worksheet, rowNumber, columnNumber, value);
        }

        public static void SetTableHeaderCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            SetHeaderCell(worksheet, rowNumber, columnNumber, value);
            worksheet.Cell(rowNumber, columnNumber).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        public static void SetTableCell(IXLWorksheet worksheet, int rowNumber, int columnNumber, XLCellValue value)
        {
            SetCellBase(worksheet, rowNumber, columnNumber, value);
            worksheet.Cell(rowNumber, columnNumber).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
    }
}

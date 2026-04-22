using ClosedXML.Excel;
using NPOI.SS.UserModel;
using System.IO;

namespace Api.Common.Helper
{
    public static class ExcelDashboardHelper
    {
        public static byte[] ExportToExcelByteArray(IWorkbook workbook)
        {
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream, false);

                var result = stream.ToArray();
                return result;
            }
        }

        public static IXLCell SetStyle(this IXLCell cell, bool isBold = false, bool isWrapText = false, XLAlignmentHorizontalValues horizontalAlignment = XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues verticalAlignment = XLAlignmentVerticalValues.Top, bool fillColor = false, ExcelDashboardHelperBorderType borderType = ExcelDashboardHelperBorderType.None, bool IsUnderLine = false)
        {
            if (isBold)
            {
                cell.Style.Font.SetBold(true);
            }
            if (isWrapText)
            {
                cell.Style.Alignment.WrapText = true;
            }
            if (IsUnderLine)
            {
                cell.Style.Font.Underline = XLFontUnderlineValues.Single;
            }
            cell.Style.Alignment.Vertical = verticalAlignment;
            cell.Style.Alignment.Horizontal = horizontalAlignment;
            if (fillColor)
            {
                cell.Style.Fill.SetBackgroundColor(XLColor.Yellow);
            }
            if (borderType != ExcelDashboardHelperBorderType.None)
            {
                switch (borderType)
                {
                    case ExcelDashboardHelperBorderType.All:
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Top:
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Bottom:
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Left:
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Right:
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        break;
                }
            }

            return cell;
        }

        public static IXLRange SetRange(this IXLWorksheet sheet, int firstRow, int lastRow, int firstCol, int lastCol, bool isBold = false, bool isWrapText = false, ExcelDashboardHelperBorderType borderType = ExcelDashboardHelperBorderType.None, bool fillColor = false, XLAlignmentHorizontalValues horizontalAlignment = XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues verticalAlignment = XLAlignmentVerticalValues.Top, bool isMerge = true)
        {
            IXLRange range = sheet.Range(firstRow, firstCol, lastRow, lastCol);
            range.Merge();
            if (fillColor)
            {
                range.Style.Fill.SetBackgroundColor(XLColor.Yellow);
            }
            if (isBold)
            {
                range.Style.Font.SetBold(true);
            }
            if (isWrapText)
            {
                range.Style.Alignment.WrapText = true;
            }
            range.Style.Alignment.Vertical = verticalAlignment;
            range.Style.Alignment.Horizontal = horizontalAlignment;
            if (borderType != ExcelDashboardHelperBorderType.None)
            {
                switch (borderType)
                {
                    case ExcelDashboardHelperBorderType.All:
                        range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Top:
                        range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Bottom:
                        range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Left:
                        range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        break;
                    case ExcelDashboardHelperBorderType.Right:
                        range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        break;
                }
            }
            if (isMerge == false)
            {
                range.Unmerge();
            }
            return range;
        }

        public enum ExcelDashboardHelperBorderType
        {
            None,
            All,
            Top,
            Bottom,
            Left,
            Right,
        }
    }
}

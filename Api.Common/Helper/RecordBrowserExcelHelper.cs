using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Api.Common.Helper
{
    public static class RecordBrowserExcelHelper
    {
        private const int ExcelMaxRowIndex = 1048575;
        private const string ValidationSheetName = "_RecordBrowserValidation";
        private const string IsUploadValidationRangeName = "_IsUploadFlagValues";
        private const string RecordCategoryValidationRangeName = "_RecordCategoryTypeValues";
        private static readonly Dictionary<string, int> ValidationRangeColumnIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { IsUploadValidationRangeName, 0 },
            { RecordCategoryValidationRangeName, 1 }
        };

        public static void ApplyIsUploadValidation(Worksheet worksheet, int columnIndex)
        {
            ApplyListValidation(worksheet, columnIndex, new[] { "Y" }, IsUploadValidationRangeName, "Please enter 'Y' or leave the cell blank.");
        }

        public static void ApplyRecordCategoryValidation(Worksheet worksheet, int columnIndex, IReadOnlyList<string> allowedValues)
        {
            ApplyListValidation(worksheet, columnIndex, allowedValues, RecordCategoryValidationRangeName, "Please select a value from the provided list.");
        }

        public static IReadOnlyList<T> MergeColumns<T>(IEnumerable<T> systemColumns, IReadOnlyList<T> columns)
        {
            if (columns == null || columns.Count == 0)
            {
                return systemColumns is IReadOnlyList<T> readOnly
                    ? readOnly
                    : systemColumns != null ? new List<T>(systemColumns) : Array.Empty<T>();
            }

            var combined = new List<T>();
            if (systemColumns != null)
            {
                combined.AddRange(systemColumns);
            }

            combined.AddRange(columns);
            return combined;
        }

        public static DateTime? ConvertToNullableDate(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is DateTime dateTime)
            {
                return dateTime;
            }

            if (value is string str && DateTime.TryParse(str, out var parsed))
            {
                return parsed;
            }

            return null;
        }

        public static void PutCellValue(Cell cell, object value)
        {
            if (cell == null)
            {
                return;
            }

            if (value == null)
            {
                cell.PutValue(string.Empty);
                return;
            }

            switch (value)
            {
                case string str:
                    cell.PutValue(str);
                    break;
                case int i:
                    cell.PutValue(i);
                    break;
                case long l:
                    cell.PutValue(l);
                    break;
                case double d:
                    cell.PutValue(d);
                    break;
                case decimal dec:
                    cell.PutValue(Convert.ToDouble(dec));
                    break;
                case bool flag:
                    cell.PutValue(flag);
                    break;
                case DateTime date:
                    cell.PutValue(date);
                    break;
                default:
                    cell.PutValue(value.ToString());
                    break;
            }
        }

        public static void ApplyContentBaseStyle(Style style)
        {
            if (style == null)
            {
                return;
            }

            style.HorizontalAlignment = TextAlignmentType.Left;
            style.VerticalAlignment = TextAlignmentType.Center;
            style.IsTextWrapped = true;
            ApplyTableBorders(style, Color.LightGray);
        }

        public static void ApplyTableBorders(Style style, Color borderColor)
        {
            if (style == null)
            {
                return;
            }

            var borders = style.Borders;
            borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            borders[BorderType.TopBorder].Color = borderColor;
            borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            borders[BorderType.BottomBorder].Color = borderColor;
            borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            borders[BorderType.LeftBorder].Color = borderColor;
            borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            borders[BorderType.RightBorder].Color = borderColor;
        }

        private static void ApplyListValidation(Worksheet worksheet, int columnIndex, IReadOnlyList<string> allowedValues, string rangeName, string errorMessage)
        {
            if (worksheet == null || columnIndex < 0)
            {
                return;
            }

            if (allowedValues == null || allowedValues.Count == 0)
            {
                return;
            }

            var formula = EnsureListValidationFormula(worksheet.Workbook, rangeName, allowedValues);
            if (string.IsNullOrEmpty(formula))
            {
                return;
            }

            var validations = worksheet.Validations;
            var area = CellArea.CreateCellArea(1, columnIndex, ExcelMaxRowIndex, columnIndex);
            var validationIndex = validations.Add(area);
            var validation = validations[validationIndex];
            validation.Type = ValidationType.List;
            validation.Operator = OperatorType.None;
            validation.Formula1 = formula;
            validation.ShowError = true;
            validation.ErrorTitle = "Invalid value";
            validation.ErrorMessage = errorMessage;
            validation.IgnoreBlank = true;
            validation.InCellDropDown = true;
        }

        private static string EnsureListValidationFormula(Workbook workbook, string rangeName, IReadOnlyList<string> values)
        {
            if (workbook == null || string.IsNullOrWhiteSpace(rangeName) || values == null || values.Count == 0)
            {
                return string.Empty;
            }

            var validationSheet = EnsureValidationSheet(workbook);
            if (!ValidationRangeColumnIndex.TryGetValue(rangeName, out var columnIndex))
            {
                columnIndex = ValidationRangeColumnIndex.Count;
                ValidationRangeColumnIndex[rangeName] = columnIndex;
            }

            for (var rowIndex = 0; rowIndex < values.Count; rowIndex++)
            {
                validationSheet.Cells[rowIndex, columnIndex].PutValue(values[rowIndex] ?? string.Empty);
            }

            var columnName = CellsHelper.ColumnIndexToName(columnIndex);
            var reference = $"={ValidationSheetName}!${columnName}$1:${columnName}${values.Count}";

            var names = workbook.Worksheets.Names;
            var namedRange = names?[rangeName];
            if (namedRange == null)
            {
                var range = validationSheet.Cells.CreateRange(0, columnIndex, values.Count, 1);
                range.Name = rangeName;
                namedRange = workbook.Worksheets.Names[rangeName];
            }

            if (namedRange != null)
            {
                namedRange.RefersTo = reference;
            }

            return $"={rangeName}";
        }

        private static Worksheet EnsureValidationSheet(Workbook workbook)
        {
            var validationSheet = workbook.Worksheets[ValidationSheetName];
            if (validationSheet == null)
            {
                var index = workbook.Worksheets.Add();
                validationSheet = workbook.Worksheets[index];
                validationSheet.Name = ValidationSheetName;
            }

            validationSheet.IsVisible = false;
            validationSheet.VisibilityType = VisibilityType.VeryHidden;
            return validationSheet;
        }
    }
}

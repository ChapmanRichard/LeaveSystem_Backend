using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Api.Common
{
    public static class ExcelHelper
    {
        #region Export
        public delegate void ExportResult(bool res);
        public static event ExportResult ExportResultEvent;

        private static HSSFWorkbook workbook = null;

        private static HSSFWorkbook Workbook
        {
            get
            {
                if (workbook == null)
                {
                    workbook = new HSSFWorkbook();
                }
                return workbook;
            }
            set { workbook = value; }
        }

        public static void ExportToFile<T>(List<T> list, string strHeaderText, string strFileName, string[] titles = null)
        {
            try
            {
                System.Data.DataTable dtSource = ListToDataTable(list);
                Export(dtSource, strHeaderText, strFileName);
                System.GC.Collect();
                ExportResultEvent?.Invoke(true);
            }
            catch (Exception)
            {
                ExportResultEvent?.Invoke(false);
            }
        }

        public static byte[] ExportToFile<T>(List<T> list, string fileName)
        {
            System.Data.DataTable dtSource = ListToDataTable(list);
            using (MemoryStream ms = Export(dtSource, fileName))
            {
                workbook = null;
                return ms.ToArray();
            }

        }

        public static void Export(System.Data.DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        private static MemoryStream Export(System.Data.DataTable dtSource, string sheetName)
        {
            try
            {
                ISheet sheet = Workbook.CreateSheet(sheetName);
                ICellStyle dateStyle = Workbook.CreateCellStyle();
                IDataFormat format = Workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                int[] arrColWidth = new int[dtSource.Columns.Count];
                foreach (DataColumn item in dtSource.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                int rowIndex = 0;
                foreach (DataRow row in dtSource.Rows)
                {
                    #region add tab，fill tab head，fill column head，style
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        //if (rowIndex != 0)
                        //{
                        //    sheet = Workbook.CreateSheet(sheetName);
                        //}

                        #region Tab Head and style
                        //{
                        //    IRow headerRow = sheet.CreateRow(0);
                        //    headerRow.HeightInPoints = 25;
                        //    headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        //    ICellStyle headStyle = Workbook.CreateCellStyle();
                        //    headStyle.Alignment = HorizontalAlignment.Center;
                        //    IFont font = Workbook.CreateFont();
                        //    font.FontHeightInPoints = 20;
                        //    font.Boldweight = 700;
                        //    headStyle.SetFont(font);
                        //    headerRow.GetCell(0).CellStyle = headStyle;
                        //    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                        //    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        //}
                        #endregion

                        #region Column Head and style
                        IRow headerRow = sheet.CreateRow(0);
                        ICellStyle headStyle = Workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = Workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        // font.Boldweight = 700; // 已过时，改用 IsBold
                        font.IsBold = true;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 8) * 256);
                        }
                        #endregion

                        rowIndex = 1;
                    }
                    #endregion

                    #region Data Format
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        ICell newCell = dataRow.CreateCell(column.Ordinal);
                        string drValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String":
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime":
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = dateStyle;
                                break;
                            case "System.Boolean":
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull":
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }
                    }
                    #endregion

                    rowIndex++;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    Workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    return ms;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new MemoryStream();
            }
        }

        public static System.Data.DataTable ListToDataTable<T>(List<T> list)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Type listType = typeof(T);
                PropertyInfo[] properties = listType.GetProperties();

                //Column Title
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    var attribute = property.GetCustomAttribute<DisplayNameAttribute>();
                    string displayName = property.Name;
                    if (attribute != null)
                    {
                        displayName = attribute.DisplayName;
                    }
                    dt.Columns.Add(new DataColumn(displayName, property.PropertyType));
                }

                //Data
                foreach (T item in list)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = properties[i].GetValue(item, null);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }

        }
        #endregion

        #region Import
        public static System.Data.DataTable ExcelToDataTable(bool isColumnName, string filePath = "")
        {
            System.Data.DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;

            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        int sheetnum = workbook.NumberOfSheets;
                        for (int s = 0; s < sheetnum; s++)
                        {
                            sheet = workbook.GetSheetAt(s);
                            if (s == 0)
                            {
                                dataTable = new System.Data.DataTable();
                                if (sheet != null)
                                {
                                    int rowCount = sheet.LastRowNum;//總行數  
                                    if (rowCount > 0)
                                    {
                                        IRow firstRow = sheet.GetRow(0);//第一行  
                                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                                        int cellCount = firstRow.LastCellNum;//列數  
                                        List<int> columns = new List<int>();
                                        //構建System.Data.DataTable的列  
                                        if (isColumnName)
                                        {
                                            startRow = 1;//如果第一行是列名，則從第二行開始讀取  
                                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                            {
                                                object obj = GetValueType(header.GetCell(i));
                                                if (obj == null || obj.ToString() == string.Empty)
                                                {
                                                    dataTable.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                                }
                                                else
                                                {
                                                    dataTable.Columns.Add(new DataColumn(obj.ToString().Replace("\n", "")));
                                                }
                                                columns.Add(i);
                                            }
                                        }
                                        else
                                        {
                                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                            {
                                                column = new DataColumn("column" + (i + 1));
                                                dataTable.Columns.Add(column);
                                            }
                                        }

                                        //填充行  
                                        for (int i = startRow; i <= rowCount; ++i)
                                        {
                                            row = sheet.GetRow(i);
                                            if (row == null) continue;

                                            dataRow = dataTable.NewRow();
                                            bool hasValue = false;
                                            foreach (int j in columns)
                                            {
                                                dataRow[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                                                if (dataRow[j] != null && dataRow[j].ToString() != string.Empty)
                                                {
                                                    hasValue = true;
                                                }
                                            }
                                            if (hasValue)
                                            {
                                                dataTable.Rows.Add(dataRow);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isColumnName = false;
                                if (sheet != null)
                                {
                                    int rowCount = sheet.LastRowNum;//總行數  
                                    if (rowCount > 0)
                                    {
                                        IRow firstRow = sheet.GetRow(0);//第一行  
                                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                                        int cellCount = firstRow.LastCellNum;//列數

                                        //填充行  
                                        for (int i = 1; i <= rowCount; ++i)
                                        {
                                            row = sheet.GetRow(i);
                                            if (row == null) continue;

                                            dataRow = dataTable.NewRow();
                                            bool hasValue = false;
                                            for (int j = 0; j < dataTable.Columns.Count; j++)
                                            {
                                                dataRow[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                                                if (dataRow[j] != null && dataRow[j].ToString() != string.Empty)
                                                {
                                                    hasValue = true;
                                                }
                                            }
                                            if (hasValue)
                                            {
                                                dataTable.Rows.Add(dataRow);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC: 
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        return cell.NumericCellValue;
                    }
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }

        public static System.Data.DataTable GetDataTableFromExcel(string filePath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(filePath);
                Aspose.Cells.WorksheetCollection worksheets = workbook.Worksheets;
                Aspose.Cells.Worksheet worksheet = null;
                Aspose.Cells.Cells cell = null;
                int rowIndex = 0;

                int colIndex = 0;
                for (int i = 0; i < worksheets.Count; i++)
                {
                    worksheet = worksheets[i];
                    //get all sheet
                    cell = worksheet.Cells;
                    dt = cell.ExportDataTableAsString(rowIndex, colIndex, cell.MaxDataRow + 1, cell.MaxDataColumn + 1, true);

                    //table
                    dt.TableName = "table" + i.ToString();
                }
                worksheets.Clear();
                worksheet = null;
                worksheets = null;
                workbook = null;
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public static System.Data.DataTable GetDataTable(string fileName, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Log.Error("File Import Error(" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "): fileName is null or empty.");
                    return new System.Data.DataTable();
                }
                if (string.IsNullOrEmpty(path))
                {
                    Log.Error("File Import Error(" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "): path is null or empty.");
                    return new System.Data.DataTable();
                }
                return fileName.Contains(".xlsx") ? ExcelHelper.GetDataTableFromExcel(path) : ExcelHelper.ExcelToDataTable(true, path);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new System.Data.DataTable();
            }
        }
        #endregion

        #region Color
        public static Color ToColor(this string color)
        {

            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }
        #endregion
    }
}

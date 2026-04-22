using System;
using System.Globalization;

namespace Api.Common.Formatter
{
    public static class DateTimeFormatter
    {
        static DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
        static CultureInfo SpecificCulture = CultureInfo.CreateSpecificCulture("en-GB");
        public const string DISPLAY_FORMAT_DATETIME = "dd-MMM-yyyy HH:mm";

        public const string DISPLAY_FORMAT_DATETIME_NORMAL = "yyyy/MM/dd HH:mm";
        public const string DISPLAY_FORMAT_DATE = "dd-MMM-yyyy";
        public const string DISPLAY_FORMAT_DATE_NOYEAR = "dd MMM";
        public const string DISPLAY_PREVIEW_FORMAT_DATE = "dd.MM.yyyy";
        public const string DISPLAY_PDF_PREVIEW_FORMAT_DATE = "dd MMM yyyy";
        public const string DISPLAY_PDF_PREVIEW_FORMAT_STR_DATE = "dd MMMM yyyy";
        public const string DISPLAY_FILE_FORMAT_DATE = "dd/MM/yyyy";
        public const string SCRIPT_FORMAT_DATETIME = "dd-MM-yyyy@HH:mm";
        public const string SCRIPT_FORMAT_DATE = "dd-MM-yyyy";
        public const string SCRIPT_FORMAT_TIME = "HH:mm";
        public const string SCRIPT_FORMAT_MONTH = "MM-yyyy";
        //public const string DISPLAY_FORMAT_DATE_Eng = "dd MMM yyyy";
        public const string DISPLAY_FORMAT_DATETIME_Eng = "dd MMM yyyy HH:mm";
        public const string DISPLAY_FORMAT_DATETIME_Eng_Full = "dd MMM yyyy HH:mm:ss";
        public const string DISPLAY_FORMAT_DATETIME_TT = "hh:mm tt";
        public const string DISPLAY_FORMAT_DATETIME_HHmm = "MM/dd/yyyy HH:mm";
        public const string DISPLAY_FORMAT_DATETIME_HHmmss = "MM/dd/yyyy HH:mm:ss";
        public const string DISPLAY_FORMAT_DATE_REPORT = "yyyy/MM/dd";
        public const string DISPLAY_FORMAT_DATE_REPORT_V2 = "dd MMM yyyy";
        public const string DISPLAY_FORMAT_DATE_REPORT_TITLE = "dd.MM.yyyy";
        public const string SCRIPT_FORMAT_YearMONTH = "MM/yyyy";
        public const string DISPLAY_FORMAT_ISSUE_DATE = "dd : MM : yyyy";
        public static string GetCurrentIssueDate()
        {
            return DateTime.Now.ToString(DISPLAY_FORMAT_ISSUE_DATE);
        }
        public static string GetFirstIssueDate(DateTime? v)
        {
            return v?.ToString(DISPLAY_FORMAT_ISSUE_DATE) ?? DateTime.Now.ToString(DISPLAY_FORMAT_ISSUE_DATE);
        }
        public static string DisplayDateTimeYearMONTH(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(SCRIPT_FORMAT_YearMONTH, myDTFI);
        }
        public static string DisplayDateTimeHHmmss(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_HHmmss, myDTFI);
        }
        public static string DisplayDateTimeHHmm(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_HHmm, myDTFI);
        }
        public static string DisplayDateTimeTT(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_TT, myDTFI);
        }
        public static string DisplayDayOfWeekEng(DateTime? v)
        {
            return v == null ? "" : myDTFI.GetDayName(v.Value.DayOfWeek);
        }
        public static string DisplayDateEng(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PDF_PREVIEW_FORMAT_DATE, myDTFI);
        }
        public static string DisplayDateTimeEng(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_Eng, myDTFI);
        }
        public static string DisplayDateTimeEngFull(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_Eng_Full, myDTFI);
        }
        public static string DisplayDateTime(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME, myDTFI);
        }
        public static string DisplayDateTimeNormal(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATETIME_NORMAL, myDTFI);
        }
        public static string DisplayDate(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATE, myDTFI);
        }
        public static string DisplayDateReport(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(SCRIPT_FORMAT_DATE, myDTFI);
        }

        public static string DisplayDateNoBar(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PDF_PREVIEW_FORMAT_DATE, myDTFI);
        }
        public static string DisplayDateNoBarFullMonth(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PDF_PREVIEW_FORMAT_STR_DATE, myDTFI);
        }
        public static string DisplayDateNoBarStr(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PDF_PREVIEW_FORMAT_STR_DATE, myDTFI);
        }
        public static string DisplayDateNoBar_Specific(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PDF_PREVIEW_FORMAT_DATE, SpecificCulture);
        }
        public static string DisplayDateNoYear(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATE_NOYEAR, myDTFI);
        }
        public static string DisplayDateForReport(DateTime? v)
        {
            return v == null ? "N.A." : v.Value.ToString(DISPLAY_FORMAT_DATE_REPORT, myDTFI);
        }
        public static string DisplayDateForReportV2(DateTime? v)
        {
            return v == null ? "-" : v.Value.ToString(DISPLAY_FORMAT_DATE_REPORT_V2, myDTFI);
        }
        public static string DisplayDateForReportTitle(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FORMAT_DATE_REPORT_TITLE, myDTFI);
        }
        public static string DisplayFileFormatDate(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_FILE_FORMAT_DATE, myDTFI);
        }
        public static string DisplayPreviewFormatDate(DateTime? v)
        {
            return v == null ? "" : v.Value.ToString(DISPLAY_PREVIEW_FORMAT_DATE, myDTFI);
        }
        public static DateTime DisplayDate(string v)
        {
            return Convert.ToDateTime(v, myDTFI);
        }

        public static DateTime? DisplayDateTime(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return null;
            }
            return Convert.ToDateTime(v, myDTFI);
        }
        public static string DisplayDateTime(DateTime? v, string datetimeformat)
        {
            return v == null ? "" : v.Value.ToString(datetimeformat, myDTFI);
        }
        public static DateTime ConvertToDateTime(string v, string format)
        {
            myDTFI.ShortDatePattern = format;
            return Convert.ToDateTime(v, myDTFI);
        }
    }
}

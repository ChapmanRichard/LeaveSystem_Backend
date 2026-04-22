using System;
using System.Globalization;

namespace Api.Common.Helper
{
    /// <summary>
    /// Kong Edit
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取当月第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthFirstDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
        }
        /// <summary>
        /// 获取当月最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthFinalDay(this DateTime dateTime)
        {
            var nextMonth = dateTime.GetMonthFirstDay().AddMonths(1);
            return nextMonth.AddSeconds(-1);
        }
        /// <summary>
        /// 获取一天得起始
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDayBeginDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取一天的结尾
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDayEndDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        /// <summary>
        /// 获取当周第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekBeginDate(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            DateTime now = dateTime.GetDayBeginDateTime();
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    now = now.AddDays(-6.0);
                    break;

                case DayOfWeek.Monday:
                    break;

                case DayOfWeek.Tuesday:
                    now = now.AddDays(-1.0);
                    break;

                case DayOfWeek.Wednesday:
                    now = now.AddDays(-2.0);
                    break;

                case DayOfWeek.Thursday:
                    now = now.AddDays(-3.0);
                    break;

                case DayOfWeek.Friday:
                    now = now.AddDays(-4.0);
                    break;

                case DayOfWeek.Saturday:
                    now = now.AddDays(-5.0);
                    break;
            }
            return new DateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// 获取每周最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekEndDate(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            DateTime now = dateTime.GetDayBeginDateTime();
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    now = now.AddDays(0.0);
                    break;

                case DayOfWeek.Monday:
                    now = now.AddDays(6.0);
                    break;

                case DayOfWeek.Tuesday:
                    now = now.AddDays(5.0);
                    break;

                case DayOfWeek.Wednesday:
                    now = now.AddDays(4.0);
                    break;

                case DayOfWeek.Thursday:
                    now = now.AddDays(3.0);
                    break;

                case DayOfWeek.Friday:
                    now = now.AddDays(2.0);
                    break;

                case DayOfWeek.Saturday:
                    now = now.AddDays(1.0);
                    break;
            }
            return new DateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// 获取当年第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearFirstDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
        }

        /// <summary>
        /// 获取当年最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearEndDateTime(this DateTime dateTime)
        {
            return dateTime.GetYearFirstDateTime().AddYears(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取当前时间戳 （10位）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 获取当前时间戳 （13位）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampLong(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 根据生日获取年龄
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        public static int GetAgeByBirthdate(this DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        /// <summary>
        /// 根据特定格式转化为DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? ConvertDateTimeByFormat(this string str, string format)
        {
            var result = new DateTime();
            if (DateTime.TryParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ConvertDateTimeByFormat(this string str, string format, CultureInfo cultureInfo)
        {
            var result = new DateTime();
            if (DateTime.TryParseExact(str, format, cultureInfo, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static string ToEnglishFullString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
        }
        public static string ToEnglishFullStringFullMonth(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
        }
        public static string ToEnglishFullStringFullMonthWithHoursAndMinutes(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy HH:mm", CultureInfo.CreateSpecificCulture("en-GB"));
        }
        public static string ToEnglishFullStringWithLine(this DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
        }
    }
}

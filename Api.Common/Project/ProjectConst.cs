using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Common
{
    public static class ProjectConst
    {
        public static T GetProjectConstValue<T>(this Dictionary<string, T> keyValues, string key)
        {
            return keyValues.Where(S => S.Key == key).Select(S => S.Value).FirstOrDefault();
        }
        public static T2 GetProjectConstValue<T1, T2>(this Dictionary<T1, T2> keyValues, T1 key) where T1 : IComparable<T1>
        {
            return keyValues.Where(S => S.Key.CompareTo(key) == 0).Select(S => S.Value).FirstOrDefault();
        }
        public static string GetProjectConstKey<T>(this Dictionary<string, T> keyValues, List<T> values)
        {
            return keyValues.Where(S => values.Contains(S.Value)).Select(S => S.Key).First();
        }
        public static string GetProjectConstKey<T>(this Dictionary<string, T> keyValues, T value) where T : IComparable<T>
        {
            return keyValues.Where(S => S.Value.CompareTo(value) == 0).Select(S => S.Key).First();
        }
        public static T1 GetProjectConstKey<T1, T2>(this Dictionary<T1, T2> keyValues, T2 value) where T2 : IComparable<T2>
        {
            return keyValues.Where(S => S.Value.CompareTo(value) == 0).Select(S => S.Key).FirstOrDefault();
        }
        public static Dictionary<string, string> CountryLookup = new Dictionary<string, string> {
            {"1","Amsterdam"},{"2","Antwerp"},{"3","Athens"},{"4","Barcelona"},{"5","Berlin"},{"6","Birmingham"},{"7","Bradford"},{"8","Bremen"},{"9","Brussels"},{"10","Bucharest"},
            {"11","Budapest"},{"12","Cologne"},{"13","Copenhagen"},{"14","Dortmund"},{"15","Dresden"},{"16","Dublin"}
        };
        public static Dictionary<string, string> CalendarCategoryLookup = new Dictionary<string, string> {
            {"1","Submission Due Date"},{"2","Meeting"},{"3","Target Submission Date"},{"4","Site Inspection Date"},{"5","On-Leave Date"},{"6","Ad-hoc Event"},{"7","Seminar"},{"8","Remind"}
        };
        public static Dictionary<string, string> Dic_CommentProperty = new Dictionary<string, string>() { { "1", "Comments given for necessary" }, { "2", "amendments" } };
        public static string NoInFormProjectType = "O";
        //public static string UserTypeC = "C";
        //public static string UserTypeD = "D";
        public static string ProjectRoleC = "CU";
        public static string ProjectRoleD = "DU";
    }
}

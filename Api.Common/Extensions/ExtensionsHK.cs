using System;

namespace Api.Common
{
    /// <summary>
    /// 检测扩展
    /// </summary>
    public static class ExtensionsHK
    {
        /// <summary>
        /// 返回香港时间
        /// </summary>
        /// <param name="value">值</param>
        public static DateTime ToHKDateTime(this string value)
        {
            return Convert.ToDateTime(value, new System.Globalization.CultureInfo("en-HK"));
        }
    }
}

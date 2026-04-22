using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Api.Common.Helper
{
    public static class ConvertHelper
    {
        public static List<int> ConvertToIntList(this List<int?> list)
        {
            var result = new List<int>();
            foreach (var item in list)
            {
                if (item.HasValue)
                {
                    result.Add(item.Value);
                }
            }
            return result;
        }

        public static List<int?> ConvertToIntListNullAble(this List<int> list)
        {
            var result = new List<int?>();
            list.ForEach(i => result.Add(i));
            return result;
        }
        /// <summary>
        /// Remove Invalid File Name Chars
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MakeValidFileName(this string path)
        {
            StringBuilder str = new StringBuilder();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (var c in path)
            {
                if (!invalidFileNameChars.Any(i => i == c))
                {
                    str.Append(c);
                }
                else
                {
                    str.Append("_");
                }
            }

            return str.ToString();
        }
    }
}

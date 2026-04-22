using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Common.DocGen
{
    internal class AttributeUtility
    {
        public static Dictionary<string, object> GetClassAttributes<T>(bool isGetCustomAttribute = false)
        {
            Dictionary<string, object> dict =
                typeof(T).GetCustomAttributes(isGetCustomAttribute).ToDictionary(a => a.GetType().Name, a => a);
            return dict;
        }
        public static Dictionary<string, List<object>> GetPropertyAttributes<T>(String propertyName, bool isGetCustomAttribute = false)
        {
            //Due to the multiple attributes on the same attribute type, use object array to keep the values
            var dict = new Dictionary<string, List<object>>();
            object[] attrs = typeof(T).GetProperty(propertyName).GetCustomAttributes(isGetCustomAttribute);
            for (var i = 0; i < attrs.Length; i++)
            {
                string key = attrs[i].GetType().Name;
                if (dict.ContainsKey(key))
                {
                    dict[key].Add(attrs[i]);
                }
                else
                    dict.Add(key, new List<object>() { attrs[i] });
            }

            return dict;
        }
    }
}

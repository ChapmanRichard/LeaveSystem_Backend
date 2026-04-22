using System;
using System.ComponentModel;
using System.Reflection;

namespace Api.Common
{
    public static class EnumTool
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumName)
        {
            string description;
            FieldInfo fieldInfo = enumName.GetType().GetField(enumName.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetDescriptAttr();
            if (attributes != null && attributes.Length > 0)
                description = attributes[0].Description;
            else
                throw new ArgumentException($"{enumName} 未能找到对应的枚举描述.", nameof(enumName));
            return description;
        }

        /// <summary>
        /// 获取枚举描述属性
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            return (DescriptionAttribute[])fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        }

        /// <summary>
        /// 通过描述获取枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static TEnum GetEnum<TEnum>(string description)
        {
            Type type = typeof(TEnum);
            foreach (FieldInfo field in type.GetFields())
            {
                DescriptionAttribute[] curDesc = field.GetDescriptAttr();
                if (curDesc != null && curDesc.Length > 0)
                {
                    if (curDesc[0].Description == description)
                        return (TEnum)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException($"{description} 未能找到对应的枚举.", nameof(description));
        }
    }
}

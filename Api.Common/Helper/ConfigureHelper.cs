using System;
using System.Reflection;

namespace Api.Common.Helper
{
    public class ConfigureHelper
    {
        public static AttachmentConfig GetAttachmentConfig(AttachmentOptions attachmentOptions, AttachmentOwnerType attachmentOwnerType)
        {
            Type t = attachmentOptions.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                if (item.Name == attachmentOwnerType.ToString())
                {
                    return (AttachmentConfig)t.GetProperty(item.Name).GetValue(attachmentOptions, null);
                }
            }
            return null;
        }
    }
}

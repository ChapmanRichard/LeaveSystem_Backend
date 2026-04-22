using Api.Common.Loggers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Api.Common.Formatter
{
    public static class HtmlTemplateFormatter
    {
        public static string ReplacePlaceholder(string template, Dictionary<string, string> templateParams)
        {
            string result = new string(template.ToCharArray());
            if (templateParams != null)
            {
                foreach (var key in templateParams.Keys)
                {
                    Regex regex = new Regex(@"\$\{" + key + @"\}");
                    if (regex.IsMatch(result))
                    {
                        result = regex.Replace(result, templateParams[key] ?? string.Empty);
                    }
                }
            }
            else
            {
                LoggerHelper.Instance.Info("Email Error:ReplacePlaceholder error, templateParams is null.");
            }
            return result;
        }
        public static string ClearHtmlCode(this string Htmlstring)
        {
            if (Htmlstring.IsEmpty())
            {
                return Htmlstring;
            }
            // 删除脚本和样式
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
            // 删除HTML标签
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            // 常见HTML实体替换
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            // 新增：处理 ndash、mdash、hellip、apos、reg、trade 等常见实体
            Htmlstring = Regex.Replace(Htmlstring, @"&(ndash|#8211);", "–", RegexOptions.IgnoreCase); // en dash
            Htmlstring = Regex.Replace(Htmlstring, @"&(mdash|#8212);", "—", RegexOptions.IgnoreCase); // em dash
            Htmlstring = Regex.Replace(Htmlstring, @"&(hellip|#8230);", "…", RegexOptions.IgnoreCase); // …
            Htmlstring = Regex.Replace(Htmlstring, @"&(apos|#39);", "'", RegexOptions.IgnoreCase); // '
            Htmlstring = Regex.Replace(Htmlstring, @"&(reg|#174);", "\u00AE", RegexOptions.IgnoreCase); // ®
            Htmlstring = Regex.Replace(Htmlstring, @"&(trade|#8482);", "\u2122", RegexOptions.IgnoreCase); // ™
            Htmlstring = Regex.Replace(Htmlstring, @"&(euro|#8364);", "€", RegexOptions.IgnoreCase); // €
            Htmlstring = Regex.Replace(Htmlstring, @"&(yen|#165);", "¥", RegexOptions.IgnoreCase); // ¥
            Htmlstring = Regex.Replace(Htmlstring, @"&(sect|#167);", "§", RegexOptions.IgnoreCase); // §
            Htmlstring = Regex.Replace(Htmlstring, @"&(para|#182);", "¶", RegexOptions.IgnoreCase); // ¶

            // 删除所有 &#数字; 形式的实体
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            //Htmlstring = Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
    }
}

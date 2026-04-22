using System;
using System.Text;
using System.Web;

namespace Api.Common.Cryptography
{
    public static class Base64Crypto
    {

        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }


        public static string Base64Encrypt(string input, Encoding encode)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(encode.GetBytes(input)));
        }


        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }


        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }

    }
}

using System.Security.Cryptography;
using System.Text;

namespace Api.Common.Helper
{
    public class HashEncryptHelper
    {
        public static string GetMD5(string myString)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(myString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}

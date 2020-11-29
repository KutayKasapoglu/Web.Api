using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Basket.Common.Helpers
{
    public class CryptoHelper
    {
        public static string CalculateMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using (var md5 = MD5.Create())
            {
                // Password bilgilerini DB'de cyrpto olarak depolamak adına bu metot çalışmaktadır

                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Base64ForUrlEncode(string str)
        {
            try
            {
                byte[] encbuff = Encoding.UTF8.GetBytes(str);
                return UrlTokenEncode(encbuff);
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static string Base64ForUrlDecode(string str)
        {
            try
            {
                byte[] decbuff = UrlTokenDecode(str);
                return Encoding.UTF8.GetString(decbuff);
            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }

        public static string UrlTokenEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (input.Length < 1)
                return String.Empty;

            string base64Str = null;
            int endPos = 0;
            char[] base64Chars = null;

            base64Str = Convert.ToBase64String(input);
            if (base64Str == null)
                return null;

            for (endPos = base64Str.Length; endPos > 0; endPos--)
            {
                if (base64Str[endPos - 1] != '=')
                {
                    break;
                }
            }

            base64Chars = new char[endPos + 1];
            base64Chars[endPos] = (char)((int)'0' + base64Str.Length - endPos);

            for (int iter = 0; iter < endPos; iter++)
            {
                char c = base64Str[iter];

                switch (c)
                {
                    case '+':
                        base64Chars[iter] = '-';
                        break;

                    case '/':
                        base64Chars[iter] = '_';
                        break;

                    case '=':
                        base64Chars[iter] = c;
                        break;

                    default:
                        base64Chars[iter] = c;
                        break;
                }
            }
            return new string(base64Chars);
        }

        public static byte[] UrlTokenDecode(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int len = input.Length;
            if (len < 1)
                return new byte[0];

            int numPadChars = (int)input[len - 1] - (int)'0';
            if (numPadChars < 0 || numPadChars > 10)
                return null;

            char[] base64Chars = new char[len - 1 + numPadChars];

            for (int iter = 0; iter < len - 1; iter++)
            {
                char c = input[iter];

                switch (c)
                {
                    case '-':
                        base64Chars[iter] = '+';
                        break;

                    case '_':
                        base64Chars[iter] = '/';
                        break;

                    default:
                        base64Chars[iter] = c;
                        break;
                }
            }

            for (int iter = len - 1; iter < base64Chars.Length; iter++)
            {
                base64Chars[iter] = '=';
            }

            return Convert.FromBase64CharArray(base64Chars, 0, base64Chars.Length);
        }
    }
}

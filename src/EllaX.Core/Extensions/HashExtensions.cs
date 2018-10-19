using System;
using System.Security.Cryptography;
using System.Text;

namespace EllaX.Core.Extensions
{
    public static class HashExtensions
    {
        public static string Md5(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        public static string Sha256(this string input)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        public static string Sha512(this string input)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}

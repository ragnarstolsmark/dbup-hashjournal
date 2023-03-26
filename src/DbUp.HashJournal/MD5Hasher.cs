using System;
using System.Security.Cryptography;
using System.Text;

namespace DbUp.HashJournal
{
    public static class MD5Hasher
    {
        public static string GetHash(string input)
        {
            using (var hasher = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hash = hasher.ComputeHash(inputBytes);
                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLowerInvariant();
            }
        }
    }
}

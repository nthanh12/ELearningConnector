using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ElearningConnector.Models.Common
{
    public static class PasswordHelper
    {
        public static string EncodePassword(string originalPassword)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            // Bytes to string
            return Regex.Replace(BitConverter.ToString(encodedBytes), "-", "").ToLower();
        }
    }
} 
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mvp24Hours.Core.Extensions
{
    public static class ConvertExtensions
    {
        public static int? ToInt(this string value, int? defaultValue = null)
        {
            if (!value.HasValue())
                return null;
            return int.TryParse(value, out int result) ? (int?)result : defaultValue;
        }

        public static long? ToLong(this string value, long? defaultValue = null)
        {
            if (!value.HasValue())
                return null;
            return long.TryParse(value, out long result) ? (long?)result : defaultValue;
        }

        public static bool? ToBoolean(this string value, bool? defaultValue = null)
        {
            if (!value.HasValue())
                return null;
            return bool.TryParse(value, out bool result) ? (bool?)result : defaultValue;
        }

        public static decimal? ToDecimal(this string value, decimal? defaultValue = null)
        {
            if (!value.HasValue())
                return null;
            return decimal.TryParse(value, out decimal result) ? (decimal?)result : defaultValue;
        }

        public static DateTime? ToDateTime(this string value, DateTime? defaultValue = null)
        {
            if (!value.HasValue())
                return null;
            return DateTime.TryParse(value, out DateTime result) ? (DateTime?)result : defaultValue;
        }

        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string Replace(this string target, List<string> oldValues, string newValue)
        {
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        public static string OnlyNumbers(this string str)
        {
            return Regex.Replace(str, @"[^\d]*", "");
        }

        public static string OnlyNumbersLetters(this string str)
        {
            return Regex.Replace(str, @"[^\dA-Za-z]*", "");
        }

        public static string OnlyLetters(this string str)
        {
            return Regex.Replace(str, @"[^A-Za-z]*", "");
        }

        public static string RemoveDiacritics(this string input)
        {
            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string ReplaceSpecialChar(this string target)
        {
            return target.RemoveDiacritics();
        }

        public static string GetSHA256Hash(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string ZipBase64(this string str)
        {
            return System.Convert.ToBase64String(str.ZipByte());
        }

        public static byte[] ZipByte(this string str)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                DeflateStream gzip = new DeflateStream(memory, CompressionMode.Compress);
                using (StreamWriter writer = new StreamWriter(gzip, System.Text.Encoding.UTF8))
                {
                    writer.Write(str);
                }
                byte[] btComp = memory.ToArray();
                return btComp;
            }
        }

        public static string UnZipBase64(this string str)
        {
            byte[] bty = System.Convert.FromBase64String(str);
            return bty.UnZip();
        }

        public static string UnZip(this byte[] bty)
        {
            using (MemoryStream inputStream = new MemoryStream(bty))
            {
                DeflateStream gzip = new DeflateStream(inputStream, CompressionMode.Decompress);
                using (StreamReader reader = new StreamReader(gzip, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

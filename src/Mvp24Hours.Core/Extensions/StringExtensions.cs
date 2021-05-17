//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
namespace Mvp24Hours.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int size)
        {
            string value = text ?? string.Empty;
            return value.Length > size ? value.Substring(0, size) : value;
        }

        public static string Reticence(this string text, int size)
        {
            string value = text ?? string.Empty;
            return value.Length > size ? value.Substring(0, size) + "..." : value;
        }

        public static string SubstringSafe(this string text, int start, int length = int.MaxValue)
        {
            if (start >= text.Length)
            {
                return string.Empty;
            }

            if (start + length > text.Length)
            {
                length = text.Length - start;
            }

            return text.Substring(start, length);
        }
    }
}

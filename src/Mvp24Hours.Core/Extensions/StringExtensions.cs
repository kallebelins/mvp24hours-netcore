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
    }
}

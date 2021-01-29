using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
                return true;
            else
                return false;
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    public static class MongoDbQueryExtensions
    {
        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                return default;
            return source.Any().TaskResult();
        }
        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                return default;
            return source.Count().TaskResult();
        }
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                return default;
            return source.SingleOrDefault().TaskResult();
        }
        public static Task<IList<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                return default;
            return ((IList<TSource>)source.ToList()).TaskResult();
        }

    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool IsList(this object Value)
        {
            var type = Value.GetType();
            return typeof(IEnumerable).IsAssignableFrom(type)
                || typeof(ICollection).IsAssignableFrom(type)
                || typeof(IList).IsAssignableFrom(type);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsList<T>(this object Value)
        {
            var type = Value.GetType();
            return typeof(IEnumerable<T>).IsAssignableFrom(type)
                || typeof(ICollection<T>).IsAssignableFrom(type)
                || typeof(IList<T>).IsAssignableFrom(type);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T t in list)
            {
                action(t);
                yield return t;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<TSource> FirstOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> task, Func<TSource, bool> predicate = null)
        {
            var list = await task;
            return predicate == null ? list.FirstOrDefault() : list.FirstOrDefault(predicate);
        }

        public static async Task<TSource> LastOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> task, Func<TSource, bool> predicate = null)
        {
            var list = await task;
            return predicate == null ? list.LastOrDefault() : list.LastOrDefault(predicate);
        }

        public static async Task<TSource> ElementAtOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> task, int index)
        {
            var list = await task;
            return list.ElementAtOrDefault(index);
        }
    }
}

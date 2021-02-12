//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class BusinessPagingExtensions
    {
        public static Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this IList<T> data, IPageResult page, ISummaryResult summary)
        {
            return Task.FromResult(ToBusinessPaging(data, page, summary));
        }

        public static IPagingResult<T> ToBusinessPaging<T>(this IList<T> data, IPageResult page, ISummaryResult summary)
        {
            return new PagingResult<T>(page, summary, new ReadOnlyCollection<T>(data ?? new List<T>()));
        }
    }
}

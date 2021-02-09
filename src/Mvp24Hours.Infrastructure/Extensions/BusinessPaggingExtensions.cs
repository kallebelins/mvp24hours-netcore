//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.DTO.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class BusinessPaggingExtensions
    {
        public static Task<IPagingResult<T>> ToBusinessPaggingAsync<T>(this IList<T> _data, IPageResult page, ISummaryResult summary)
        {
            return Task.FromResult(ToBusinessPagging(_data, page, summary));
        }

        public static IPagingResult<T> ToBusinessPagging<T>(this IList<T> _data, IPageResult page, ISummaryResult summary)
        {
            return new PagingResult<T>(page, summary, _data);
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BusinessPagingAsyncExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this Task<T> valueAsync, IPageResult page, ISummaryResult summary, IList<IMessageResult> messageResult = null, string tokenDefault = null)
        {
            T value = default;
            if (valueAsync != null)
            {
                value = await valueAsync;
            }
            return new PagingResult<T>(
                page,
                summary,
                value,
                messages: new ReadOnlyCollection<IMessageResult>(messageResult ?? new List<IMessageResult>()),
                token: tokenDefault
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this Task<T> valueAsync, IList<IMessageResult> messageResult, string tokenDefault = null)
        {
            T value = default;
            if (valueAsync != null)
            {
                value = await valueAsync;
            }
            return new PagingResult<T>(
                new PageResult(0, 0, 0),
                new SummaryResult(0, 0),
                data: value,
                messages: new ReadOnlyCollection<IMessageResult>(messageResult ?? new List<IMessageResult>()),
                token: tokenDefault
            );
        }
    }
}

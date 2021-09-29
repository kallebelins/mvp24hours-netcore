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
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class BusinessPagingExtensions
    {
        public static Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this IList<T> data, IPageResult page, ISummaryResult summary, string tokenDefault = null)
        {
            return Task.FromResult(ToBusinessPaging(data, page, summary, tokenDefault));
        }

        public static IPagingResult<T> ToBusinessPaging<T>(this IList<T> data, IPageResult page, ISummaryResult summary, string tokenDefault = null)
        {
            return new PagingResult<T>(
                page,
                summary,
                new ReadOnlyCollection<T>(data ?? new List<T>()),
                token: tokenDefault
            );
        }

        public static IPagingResult<T> ToBusinessPagingWithMessage<T>(this IList<T> data, IPageResult page, ISummaryResult summary, params IMessageResult[] messageResult)
        {
            return new PagingResult<T>(
                page,
                summary,
                new ReadOnlyCollection<T>(data ?? new List<T>()),
                messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
            );
        }

        public static IPagingResult<T> ToBusinessPagingWithMessage<T>(this IList<T> value, params IMessageResult[] messageResult)
        {
            return ToBusinessPagingWithMessage(value, null, messageResult);
        }

        public static IPagingResult<T> ToBusinessPagingWithMessage<T>(this IList<T> value, string tokenDefault = null, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new PagingResult<T>(
                    new PageResult(0, 0, 0),
                    new SummaryResult(0, 0),
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(value),
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return PagingResult<T>.Create(tokenDefault);
        }

        public static IPagingResult<T> ToBusinessPagingWithMessage<T>(this T value, params IMessageResult[] messageResult)
        {
            return ToBusinessPagingWithMessage(value, null, messageResult);
        }

        public static IPagingResult<T> ToBusinessPagingWithMessage<T>(this T value, string tokenDefault = null, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new PagingResult<T>(
                    new PageResult(0, 0, 0),
                    new SummaryResult(0, 0),
                    token: tokenDefault,
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return PagingResult<T>.Create(tokenDefault);
        }
    }
}

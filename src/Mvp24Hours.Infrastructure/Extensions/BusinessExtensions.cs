//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class BusinessExtensions
    {
        /// <summary>
        /// Transform a message into a business object
        /// </summary>
        public static IBusinessResult<T> ToBusiness<T>(this IPipelineMessage message)
        {
            if (message != null)
            {
                IList<T> dataList = new List<T>();

                if (message.HasContent<T>())
                {
                    var itemContent = message.GetContent<T>();
                    if (itemContent != null)
                    {
                        dataList.Add((T)itemContent);
                    }
                }

                IEnumerable<T> items = null;

                if (message.HasContent<IList<T>>())
                    items = message.GetContent<IList<T>>();

                if (message.HasContent<ICollection<T>>())
                    items = message.GetContent<ICollection<T>>();

                if (message.HasContent<IEnumerable<T>>())
                    items = message.GetContent<IEnumerable<T>>();

                if (items != null && items.Any())
                {
                    foreach (var item in items)
                        dataList.Add(item);
                }

                return new BusinessResult<T>(
                    token: message.Token,
                    data: new ReadOnlyCollection<T>(dataList),
                    messages: new ReadOnlyCollection<IMessageResult>(message.Messages ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>(token: message.Token);
        }
        /// <summary>
        /// Encapsulates object for business
        /// </summary>
        public static IBusinessResult<T> ToBusiness<T>(this T value)
        {
            if (value != null)
            {
                return new BusinessResult<T>(data: new ReadOnlyCollection<T>(new List<T> { value }));
            }
            return new BusinessResult<T>();
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this T value, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    data: new ReadOnlyCollection<T>(new List<T> { value }), 
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>();
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this IBusinessResult<T> value, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>();
        }
    }
}

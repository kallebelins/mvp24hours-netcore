//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
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
        public static IBusinessResult<T> ToBusiness<T>(this IPipelineMessage message, string tokenDefault = null)
        {
            if (message != null)
            {
                IList<T> dataList = new List<T>();

                if (message.HasContent<T>())
                {
                    var itemContent = message.GetContent<T>();
                    if (itemContent != null)
                    {
                        dataList.Add(itemContent);
                    }
                }

                return new BusinessResult<T>(
                    token: message.Token ?? tokenDefault,
                    data: new ReadOnlyCollection<T>(dataList),
                    messages: new ReadOnlyCollection<IMessageResult>(message.Messages ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }

        /// <summary>
        /// Encapsulates object for business
        /// </summary>
        public static IBusinessResult<T> ToBusiness<T>(this T value, string tokenDefault = null)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(new List<T> { value })
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }

        /// <summary>
        /// Encapsulates object for business
        /// </summary>
        public static IBusinessResult<T> ToBusiness<T>(this IList<T> value, string tokenDefault = null)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(value)
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this T value, params IMessageResult[] messageResult)
        {
            return ToBusinessWithMessage(value, null, messageResult);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this IList<T> value, params IMessageResult[] messageResult)
        {
            return ToBusinessWithMessage(value, null, messageResult);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this T value, string tokenDefault = null, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(new List<T> { value }),
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this IList<T> value, string tokenDefault = null, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(value),
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this IBusinessResult<T> value, params IMessageResult[] messageResult)
        {
            return ToBusinessWithMessage(value, null, messageResult);
        }

        /// <summary>
        /// Encapsulates object for business with message
        /// </summary>
        public static IBusinessResult<T> ToBusinessWithMessage<T>(this IBusinessResult<T> value, string tokenDefault = null, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    token: tokenDefault,
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>(token: tokenDefault);
        }
    }
}

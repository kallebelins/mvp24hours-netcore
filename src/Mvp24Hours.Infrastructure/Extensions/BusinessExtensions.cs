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
                var dataList = new List<T>();

                var item = message.GetContent<T>();
                if (item != null)
                {
                    dataList.Add((T)item);
                }

                IList<T> items = message.GetContent<List<T>>() ?? (IList<T>)message.GetContent<IEnumerable<T>>();
                if (items?.Count > 0)
                {
                    items?.ToList()?.ForEach(item => dataList.Add(item));
                }

                return new BusinessResult<T>(
                    token: message.Token,
                    data: new ReadOnlyCollection<T>(dataList),
                    messages: new ReadOnlyCollection<IMessageResult>(message.Messages)
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
    }
}

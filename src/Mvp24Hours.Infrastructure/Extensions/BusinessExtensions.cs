//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
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
            IBusinessResult<T> bo = new BusinessResult<T>(message.Token);
            if (message != null)
            {
                var item = message.GetContent<T>();
                if (item != null)
                {
                    bo.Data.Add((T)item);
                }
                IList<T> items = message.GetContent<List<T>>() ?? (IList<T>)message.GetContent<IEnumerable<T>>();
                if (items?.Count > 0)
                {
                    items?.ToList()?.ForEach(item => bo.Data.Add(item));
                }
                message.Messages?.ToList()?.ForEach(item => bo.Messages.Add(item));
            }
            return bo;
        }
        /// <summary>
        /// Encapsulates object for business
        /// </summary>
        public static IBusinessResult<T> ToBusiness<T>(this T value)
        {
            IBusinessResult<T> bo = new BusinessResult<T>();
            if (value != null)
            {
                bo.Data.Add(value);
            }
            return bo;
        }
    }
}

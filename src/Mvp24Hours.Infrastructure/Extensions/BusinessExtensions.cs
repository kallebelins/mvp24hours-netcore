using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.DTO.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class BusinessExtensions
    {
        public static IBusinessResult<T> ToBusiness<T>(this IPipelineMessage message)
        {
            IBusinessResult<T> bo = new BusinessResult<T>();
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
                message.Errors?.ToList()?.ForEach(item => bo.Errors.Add(new ErrorResult("PipelineMessage", item)));
            }
            bo.Token = message.Token;
            return bo;
        }

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

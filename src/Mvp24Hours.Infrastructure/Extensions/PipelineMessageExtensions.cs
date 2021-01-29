using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class PipelineMessageExtensions
    {
        public static IPipelineMessage ToMessage<T>(this T value)
        {
            IPipelineMessage message = new PipelineMessage();
            if (value != null)
            {
                message.AddContent((T)value);
            }
            return message;
        }

        public static IPipelineMessage ToMessage<T>(IBusinessResult<T> bo)
        {
            IPipelineMessage message = new PipelineMessage();
            if (bo != null)
            {
                if (bo.Data?.Count > 0)
                {
                    foreach (var item in bo.Data)
                    {
                        message.AddContent((T)item);
                    }
                }
            }
            return message;
        }

        public static IPipelineMessage ToMessageClone<T>(IBusinessResult<T> bo)
        {
            IPipelineMessage message = new PipelineMessage();
            if (bo != null)
            {
                if (bo.Data?.Count > 0)
                {
                    foreach (var item in bo.Data)
                    {
                        message.AddContent(ObjectHelper.Clone<T>(item));
                    }
                }
            }
            return message;
        }
    }
}

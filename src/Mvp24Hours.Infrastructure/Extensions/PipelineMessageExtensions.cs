//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class PipelineMessageExtensions
    {
        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(this T value)
        {
            IPipelineMessage message = new PipelineMessage();
            if (value != null)
            {
                message.AddContent((T)value);
            }
            return message;
        }
        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(this T value, string keyContent)
        {
            IPipelineMessage message = new PipelineMessage();
            if (value != null)
            {
                message.AddContent(keyContent, (T)value);
            }
            return message;
        }
        /// <summary>
        /// Transform business object to pipeline message
        /// </summary>
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
        /// <summary>
        /// Transform business object to pipeline message and clone content
        /// </summary>
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

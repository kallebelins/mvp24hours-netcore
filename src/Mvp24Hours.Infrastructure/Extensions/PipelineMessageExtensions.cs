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
        public static IPipelineMessage ToMessage<T>(this T value, string tokenDefault = null)
        {
            IPipelineMessage message = new PipelineMessage();
            if (value != null)
            {
                message.AddContent((T)value);
            }
            if (!string.IsNullOrEmpty(tokenDefault))
            {
                message.SetToken(tokenDefault);
            }
            return message;
        }
        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessageWithKeyContent<T>(this T value, string keyContent, string tokenDefault = null)
        {
            IPipelineMessage message = new PipelineMessage();
            if (value != null)
            {
                message.AddContent(keyContent, (T)value);
            }
            if (!string.IsNullOrEmpty(tokenDefault))
            {
                message.SetToken(tokenDefault);
            }
            return message;
        }
        /// <summary>
        /// Transform business object to pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(IBusinessResult<T> bo, string tokenDefault = null)
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
                if (!string.IsNullOrEmpty(bo.Token))
                {
                    message.SetToken(bo.Token);
                }
                else if (!string.IsNullOrEmpty(tokenDefault))
                {
                    message.SetToken(tokenDefault);
                }
            }
            return message;
        }
        /// <summary>
        /// Transform business object to pipeline message and clone content
        /// </summary>
        public static IPipelineMessage ToMessageClone<T>(IBusinessResult<T> bo, string tokenDefault = null)
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
                if (!string.IsNullOrEmpty(bo.Token))
                {
                    message.SetToken(bo.Token);
                }
                else if (!string.IsNullOrEmpty(tokenDefault))
                {
                    message.SetToken(tokenDefault);
                }
            }
            return message;
        }
    }
}

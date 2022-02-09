//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Pipe;
using System.Collections.Generic;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PipelineMessageExtensions
    {
        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(this T value)
        {
            return ToMessageWithToken<T>(value, null);
        }

        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessageWithToken<T>(this T value, string tokenDefault)
        {
            IPipelineMessage message = new PipelineMessage(tokenDefault);
            if (value != null)
            {
                message.AddContent(value);
            }
            return message;
        }

        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(this T value, string keyContent)
        {
            return ToMessageWithToken<T>(value, keyContent, null);
        }

        /// <summary>
        /// Encapsulates object for pipeline message
        /// </summary>
        public static IPipelineMessage ToMessageWithToken<T>(this T value, string keyContent, string tokenDefault)
        {
            IPipelineMessage message = new PipelineMessage(tokenDefault);
            if (value != null)
            {
                message.AddContent(keyContent, value);
            }
            return message;
        }

        /// <summary>
        /// Transform business object to pipeline message
        /// </summary>
        public static IPipelineMessage ToMessage<T>(IBusinessResult<T> bo)
        {
            return ToMessageWithToken<T>(bo, null);
        }

        /// <summary>
        /// Transform business object to pipeline message
        /// </summary>
        public static IPipelineMessage ToMessageWithToken<T>(IBusinessResult<T> bo, string tokenDefault)
        {
            IPipelineMessage message = new PipelineMessage(bo?.Token ?? tokenDefault);
            if (bo != null)
            {
                if (bo.Data != null)
                {
                    if (bo.Data.IsList<T>())
                    {
                        foreach (var item in bo.Data as IEnumerable<T>)
                        {
                            message.AddContent(item);
                        }
                    }
                    else
                    {
                        message.AddContent(bo.Data);
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
            return ToMessageCloneWithToken<T>(bo, null);
        }

        /// <summary>
        /// Transform business object to pipeline message and clone content
        /// </summary>
        public static IPipelineMessage ToMessageCloneWithToken<T>(IBusinessResult<T> bo, string tokenDefault)
        {
            IPipelineMessage message = new PipelineMessage(bo?.Token ?? tokenDefault);
            if (bo != null)
            {
                if (bo.Data != null)
                {
                    if (bo.Data.IsList<T>())
                    {
                        foreach (var item in bo.Data as IEnumerable<T>)
                        {
                            message.AddContent(ObjectHelper.Clone<T>(item));
                        }
                    }
                    else
                    {
                        message.AddContent(ObjectHelper.Clone<T>(bo.Data));
                    }
                }
            }
            return message;
        }
    }
}

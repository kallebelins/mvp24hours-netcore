//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Enums.Infrastructure;
using System;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine
    /// </summary>
    /// <example>
    /// <code>
    ///     IPipeline pipeline = new Pipeline();
    ///     pipeline.Add(new FileLogWriteOperation());
    ///     pipeline.Execute(filter.ToMessage());
    ///     return pipeline.GetMessage().ToBusiness{Product}();
    /// </code>
    /// </example>
    public interface IPipeline
    {
        /// <summary>
        /// Get message package
        /// </summary>
        /// <returns></returns>
        IPipelineMessage GetMessage();
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add<T>() where T : IOperation;
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add(IOperation operation);
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add(Action<IPipelineMessage> action, bool isRequired = false);
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors<T>(PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation) where T : IOperation;
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors(IOperation operation, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation);
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors(Action<IPipelineMessage> action, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation);
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors<T>(Func<IPipelineMessage, bool> condition, bool postOperation = true) where T : IOperation;
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors(IOperation operation, Func<IPipelineMessage, bool> condition, bool postOperation = true);
        /// <summary>
        /// Records operations interceptors
        /// </summary>
        IPipeline AddInterceptors(Action<IPipelineMessage> action, Func<IPipelineMessage, bool> condition, bool postOperation = true);
        /// <summary>
        /// Records event operations interceptors
        /// </summary>
        IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation);
        /// <summary>
        /// Records event operations interceptors
        /// </summary>
        IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, Func<IPipelineMessage, bool> condition, bool postOperation = true);
        /// <summary>
        /// Performs operations 
        /// </summary>
        IPipeline Execute(IPipelineMessage input = null);
    }
}

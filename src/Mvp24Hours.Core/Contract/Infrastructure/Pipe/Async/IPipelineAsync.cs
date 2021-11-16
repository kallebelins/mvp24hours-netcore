//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine async
    /// </summary>
    /// <example>
    /// <code>
    ///     IPipelineAsync pipeline = new PipelineAsync();
    ///     pipeline.AddAsync(new FileLogWriteOperation());
    ///     var result = await pipeline.Execute(filter.ToMessage());
    ///     return result.ToBusiness{Product}();
    /// </code>
    /// </example>
    public interface IPipelineAsync
    {
        /// <summary>
        /// Records async operations 
        /// </summary>
        IPipelineAsync AddAsync(IOperationAsync operation);
        /// <summary>
        /// Records async operations
        /// </summary>
        IPipelineAsync AddAsync(Action<IPipelineMessage> action, bool isRequired = false);
        /// <summary>
        /// Records async operations
        /// </summary>
        IPipelineAsync AddAsync<T>() where T : IOperationAsync;
        /// <summary>
        /// Records async operations interceptors
        /// </summary>
        IPipelineAsync AddInterceptorsAsync(IOperationAsync operation, bool postOperation = false);
        /// <summary>
        /// Records async operations interceptors
        /// </summary>
        IPipelineAsync AddInterceptorsAsync(Action<IPipelineMessage> action, bool postOperation = false);
        /// <summary>
        /// Records async operations interceptors
        /// </summary>
        IPipelineAsync AddInterceptorsAsync<T>(bool postOperation = false) where T : IOperationAsync;
        /// <summary>
        /// Performs async operations
        /// </summary>
        Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input = null);
    }
}

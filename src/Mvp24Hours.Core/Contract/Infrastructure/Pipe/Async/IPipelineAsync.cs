//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine
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
        /// Records operations
        /// </summary>
        IPipelineAsync AddAsync(IOperationAsync operation);
        /// <summary>
        /// Records operations
        /// </summary>
        IPipelineAsync AddAsync<T>() where T : IOperationAsync, new();
        /// <summary>
        /// Performs operations
        /// </summary>
        Task<IPipelineMessage> Execute(IPipelineMessage input);
    }
}

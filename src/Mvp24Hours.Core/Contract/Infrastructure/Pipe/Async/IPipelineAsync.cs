//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine
    /// </summary>
    public interface IPipelineAsync
    {
        /// <summary>
        /// Records operations
        /// </summary>
        IPipelineAsync AddAsync(IOperationAsync operation);
        /// <summary>
        /// Performs operations
        /// </summary>
        Task<IPipelineMessage> Execute(IPipelineMessage input);
    }
}

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
    /// Used to asynchronously map object
    /// </summary>
    public interface IOperationMapperAsync<T> : IOperationAsync
    {
        /// <summary>
        /// Used to asynchronously map an object
        /// </summary>
        Task<T> MapperAsync(IPipelineMessage input);
    }
}

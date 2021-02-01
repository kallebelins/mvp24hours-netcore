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

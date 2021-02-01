namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Operation for object mapping
    /// </summary>
    public interface IOperationMapper<T> : IOperation
    {
        /// <summary>
        /// Used to map object
        /// </summary>
        T Mapper(IPipelineMessage input);
    }
}

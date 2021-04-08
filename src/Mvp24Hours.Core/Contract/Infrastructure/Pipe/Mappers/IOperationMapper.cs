//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
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

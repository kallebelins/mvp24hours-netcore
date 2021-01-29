//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add(IOperation operation);
        /// <summary>
        /// Performs operations
        /// </summary>
        IPipelineMessage Execute(IPipelineMessage input);
    }
}

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
    /// Operations interface
    /// </summary>
    public interface IOperation
    {
        /// <summary>  
        /// Perform an operation
        /// </summary>
        IPipelineMessage Execute(IPipelineMessage input);

        /// <summary>
        /// Indicates whether operation is mandatory (even with failure)
        /// </summary>
        public bool IsRequired { get; }
    }
}

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
    /// Operations interface
    /// </summary>
    public interface IOperationAsync
    {
        /// <summary>  
        /// Perform an operation
        /// </summary>
        Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input);
        /// <summary>
        /// Indicates whether operation is mandatory (even with failure)
        /// </summary>
        public bool IsRequired { get; }
    }
}

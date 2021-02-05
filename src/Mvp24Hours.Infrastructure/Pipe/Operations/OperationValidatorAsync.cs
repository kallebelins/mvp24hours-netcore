//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationValidatorAsync : OperationBaseAsync
    {
        public override async Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            if (!await IsValid(input))
                input.Lock();
            return input;
        }

        public abstract Task<bool> IsValid(IPipelineMessage input);
    }
}

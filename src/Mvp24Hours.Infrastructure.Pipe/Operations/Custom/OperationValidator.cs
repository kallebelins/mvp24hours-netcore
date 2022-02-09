//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationValidator : OperationBase
    {
        public override IPipelineMessage Execute(IPipelineMessage input)
        {
            if (!IsValid(input))
            {
                input.SetLock();
            }

            return input;
        }

        public abstract bool IsValid(IPipelineMessage input);
    }
}

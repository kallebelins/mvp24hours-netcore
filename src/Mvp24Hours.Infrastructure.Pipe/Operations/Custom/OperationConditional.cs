//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Extensions;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction for conditional operation
    /// </summary>
    public abstract class OperationConditional : OperationBase
    {
        #region [ Ctors ]
        public OperationConditional() { }

        [ActivatorUtilitiesConstructor]
        public OperationConditional(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        public override IPipelineMessage Execute(IPipelineMessage input)
        {
            if (Condition(input))
            {
                TrueResult(input);
            }
            else
            {
                FalseResult(input);
            }
            return input;
        }

        public abstract bool Condition(IPipelineMessage input);
        public virtual void TrueResult(IPipelineMessage input) { }
        public virtual void FalseResult(IPipelineMessage input) { }
    }
}

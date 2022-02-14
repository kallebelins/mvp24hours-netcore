//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationValidator : OperationBase
    {
        #region [ Ctors ]
        public OperationValidator() { }

        [ActivatorUtilitiesConstructor]
        public OperationValidator(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        public override void Execute(IPipelineMessage input)
        {
            if (!IsValid(input))
            {
                input.SetLock();
            }
        }

        public abstract bool IsValid(IPipelineMessage input);
    }
}

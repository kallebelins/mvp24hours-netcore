//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationValidatorAsync : OperationBaseAsync
    {
        #region [ Ctors ]
        public OperationValidatorAsync() { }

        [ActivatorUtilitiesConstructor]
        public OperationValidatorAsync(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        public override async Task ExecuteAsync(IPipelineMessage input)
        {
            if (!await IsValid(input))
            {
                input.SetLock();
            }
        }

        public abstract Task<bool> IsValid(IPipelineMessage input);
    }
}

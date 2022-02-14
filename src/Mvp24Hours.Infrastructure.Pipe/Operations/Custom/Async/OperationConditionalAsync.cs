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
    /// Abstraction for conditional operation
    /// </summary>
    public abstract class OperationConditionalAsync : OperationBaseAsync
    {
        #region [ Ctors ]
        protected OperationConditionalAsync() { }

        [ActivatorUtilitiesConstructor]
        protected OperationConditionalAsync(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        public override async Task ExecuteAsync(IPipelineMessage input)
        {
            if (await ConditionAsync(input))
            {
                await TrueResultAsync(input);
            }
            else
            {
                await FalseResultAsync(input);
            }
        }

        public abstract Task<bool> ConditionAsync(IPipelineMessage input);
        public virtual async Task TrueResultAsync(IPipelineMessage input) { await Task.CompletedTask; }
        public virtual async Task FalseResultAsync(IPipelineMessage input) { await Task.CompletedTask; }
    }
}

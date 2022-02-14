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
    /// Abstraction of asynchronous mapping operations
    /// </summary>
    public abstract class OperationMapperAsync<T> : OperationBaseAsync
    {
        #region [ Ctors ]
        public OperationMapperAsync() { }

        [ActivatorUtilitiesConstructor]
        public OperationMapperAsync(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        /// <summary>
        /// Key defined for content attached to the message (mapped object)
        /// </summary>
        public virtual string ContentKey => null;

        public override async Task ExecuteAsync(IPipelineMessage input)
        {
            var result = await MapperAsync(input);
            if (result != null)
            {
                if (string.IsNullOrEmpty(ContentKey))
                {
                    input.AddContent(result);
                }
                else
                {
                    input.AddContent(ContentKey, result);
                }
            }
        }

        public abstract Task<T> MapperAsync(IPipelineMessage input);
    }
}

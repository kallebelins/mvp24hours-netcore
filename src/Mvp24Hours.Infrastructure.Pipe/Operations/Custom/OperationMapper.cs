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
    public abstract class OperationMapper<T> : OperationBase
    {
        #region [ Ctors ]
        public OperationMapper() { }

        [ActivatorUtilitiesConstructor]
        public OperationMapper(INotificationContext _notificationContext)
            : base(_notificationContext) { }
        #endregion

        /// <summary>
        /// Key defined for content attached to the message (mapped object)
        /// </summary>
        public virtual string ContentKey => null;

        public override IPipelineMessage Execute(IPipelineMessage input)
        {
            var result = Mapper(input);
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
            return input;
        }

        public abstract T Mapper(IPipelineMessage input);
    }
}

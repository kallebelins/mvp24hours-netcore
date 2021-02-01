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
    /// Abstraction of asynchronous mapping operations
    /// </summary>
    public abstract class OperationMapperAsync<T> : OperationBaseAsync, IOperationMapperAsync<T>
    {
        /// <summary>
        /// Key defined for content attached to the message (mapped object)
        /// </summary>
        public virtual string MessageContentKey => null;

        public override async Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            var result = await MapperAsync(input);
            if (result != null)
            {
                if (string.IsNullOrEmpty(MessageContentKey))
                {
                    input.AddContent(result);
                }
                else
                {
                    input.AddContent(MessageContentKey, result);
                }
            }
            return input;
        }

        public abstract Task<T> MapperAsync(IPipelineMessage input);
    }
}

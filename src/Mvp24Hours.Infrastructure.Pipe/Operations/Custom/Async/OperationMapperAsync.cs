//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of asynchronous mapping operations
    /// </summary>
    public abstract class OperationMapperAsync<T> : OperationBaseAsync
    {
        /// <summary>
        /// Key defined for content attached to the message (mapped object)
        /// </summary>
        public virtual string ContentKey => null;

        /// <summary>
        /// Get default value for object mapping
        /// </summary>
        protected virtual Task<T> GetDefaultValue()
        {
            return Task.FromResult<T>(default);
        }

        public override async Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
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
            return input;
        }

        public abstract Task<T> MapperAsync(IPipelineMessage input);
    }
}

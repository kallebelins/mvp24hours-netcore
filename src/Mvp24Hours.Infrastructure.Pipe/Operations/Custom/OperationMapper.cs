//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationMapper<T> : OperationBase
    {
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

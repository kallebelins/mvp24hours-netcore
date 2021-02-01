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
        public override async Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            input.AddContent(await MapperAsync(input));
            return input;
        }

        public abstract Task<T> MapperAsync(IPipelineMessage input);
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Abstraction of asynchronous mapping operations
    /// </summary>
    public abstract class OperationMapperAsync<T> : OperationBaseAsync, IOperationMapperAsync<T>
    {
        public abstract T Mapper(params object[] data);
    }
}

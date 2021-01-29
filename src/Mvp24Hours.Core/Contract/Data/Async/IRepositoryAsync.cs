//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================

using Mvp24Hours.Core.Contract.Domain.Entity;

namespace Mvp24Hours.Core.Contract.Data
{
    public interface IRepositoryAsync<T> : IQueryAsync<T>, ICommandAsync<T>
        where T : IEntityBase
    {
    }
}

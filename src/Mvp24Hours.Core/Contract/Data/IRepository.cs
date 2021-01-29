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
    public interface IRepository<T> : IQuery<T>, ICommand<T>
        where T : IEntityBase
    {
    }
}

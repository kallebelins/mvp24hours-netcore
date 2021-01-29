using System;

namespace Mvp24Hours.Core.Contract.Domain.Entity
{
    public interface IEntityLog<T>
    {
        DateTime Created { get; set; }
        T CreatedBy { get; set; }
        DateTime? Modified { get; set; }
        T ModifiedBy { get; set; }
        DateTime? Removed { get; set; }
        T RemovedBy { get; set; }
    }
}

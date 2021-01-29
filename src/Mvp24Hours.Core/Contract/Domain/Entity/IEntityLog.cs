using System;

namespace Mvp24Hours.Core.Contract.Domain.Entity
{
    public interface IEntityLog<T> : IEntityDateLog
    {
        T CreatedBy { get; set; }
        T ModifiedBy { get; set; }
        T RemovedBy { get; set; }
    }
}

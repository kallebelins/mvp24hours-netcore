using System;

namespace Mvp24Hours.Core.Contract.Domain.Entity
{
    public interface IEntityDateLog
    {
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
        DateTime? Removed { get; set; }
    }
}

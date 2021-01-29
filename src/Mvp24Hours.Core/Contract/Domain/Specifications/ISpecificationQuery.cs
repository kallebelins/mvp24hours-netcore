using System;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Domain.Specifications
{
    public interface ISpecificationQuery<T>
    {
        Expression<Func<T, bool>> IsSatisfiedByExpression { get; }
    }
}

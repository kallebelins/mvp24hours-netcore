//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Domain.Specifications
{
    /// <summary>
    /// Specification for queries
    ///  <see cref="Mvp24Hours.Core.Contract.Domain.Specifications.ISpecification{T}"/>
    /// </summary>
    public interface ISpecificationQuery<T> : ISpecification<T>
    {
        /// <summary>
        /// Lambda expression that represents a filter applied to an entity or list
        /// </summary>
        Expression<Func<T, bool>> IsSatisfiedByExpression { get; }
    }
}

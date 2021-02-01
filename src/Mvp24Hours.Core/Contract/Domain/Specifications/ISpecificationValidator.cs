//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.Domain.Specifications
{
    /// <summary>
    /// Specification for recording missed validation result
    ///  <see cref="Mvp24Hours.Core.Contract.Domain.Specifications.ISpecification{T}"/>
    /// </summary>
    public interface ISpecificationValidator<T> : ISpecification<T>
    {
        /// <summary>
        /// Validation reference (key)
        /// </summary>
        string KeyValidation { get; }
        /// <summary>
        /// Message for unanswered validation
        /// </summary>
        string MessageValidation { get; }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;

namespace Mvp24Hours.Core.Contract.Domain.Validations
{
    /// <summary>
    /// Specification testing notification manager
    /// </summary>
    /// <typeparam name="T">Test model</typeparam>
    /// <example>
    /// <code>
    ///     var validator = new ValidatorEntityNotify{Product}()
    ///         .AddSpecification{SpecialCategoryAllowsOneProductSpecification}();
    ///         .AddSpecification{IsNotSpecialCategorySpecification}()
    ///         .AddSpecification{CategoryHasNotProductSpecification}()
    ///     if (!validator.Validate(model))
    ///         return 0;
    /// </code>
    /// </example>
    public interface IValidatorNotify<T> : IValidator<T>
    {
        /// <summary>
        /// Notification context
        /// </summary>
        INotificationContext Context { get; }
        /// <summary>
        /// Indicates whether the notification context is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>()
            where U : ISpecification<T>, new();
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <param name="keyValidation">Reference key for specification not satisfied</param>
        /// <param name="messageValidation">Message for specification not satisfied</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>(string keyValidation, string messageValidation)
            where U : ISpecification<T>, new();
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <param name="specification">Specification for testing model</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification(ISpecification<T> specification);
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <param name="specification">Validation specification</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification(ISpecificationValidator<T> specification);
    }
}

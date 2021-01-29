using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;

namespace Mvp24Hours.Core.Contract.Domain.Validations
{
    public interface IValidatorNotify<T>
    {
        INotificationContext Context { get; }
        bool IsValid { get; }

        IValidatorNotify<T> AddSpecification<U>()
            where U : ISpecification<T>, new();
        IValidatorNotify<T> AddSpecification<U>(string keyValidation, string messageValidation)
            where U : ISpecification<T>, new();
        IValidatorNotify<T> AddSpecification<U>(ISpecification<T> specification)
            where U : ISpecification<T>;
        IValidatorNotify<T> AddSpecification<U>(ISpecificationValidator<T> specificationValidator)
            where U : ISpecificationValidator<T>;

        bool Validate(T Candidate);
    }
}

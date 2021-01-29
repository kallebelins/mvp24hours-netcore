namespace Mvp24Hours.Core.Contract.Domain.Specifications
{
    public interface ISpecificationValidator<T> : ISpecification<T>
    {
        string KeyValidation { get; }
        string MessageValidation { get; }
    }
}

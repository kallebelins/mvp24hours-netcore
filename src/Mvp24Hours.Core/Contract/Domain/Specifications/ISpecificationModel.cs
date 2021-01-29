namespace Mvp24Hours.Core.Contract.Domain.Specifications
{
    public interface ISpecificationModel<T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}

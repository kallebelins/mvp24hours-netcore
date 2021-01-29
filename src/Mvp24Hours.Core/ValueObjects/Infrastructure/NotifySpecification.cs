using Mvp24Hours.Core.Contract.Domain.Specifications;
using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    public class NotifySpecification<T> : BaseVO
    {
        public NotifySpecification(ISpecification<T> specification)
        {
            Specification = specification;
        }

        public NotifySpecification(string keyValidation, string messageValidation, ISpecification<T> specification)
            : this(specification)
        {
            KeyValidation = keyValidation;
            MessageValidation = messageValidation;
        }

        public string KeyValidation { get; }
        public string MessageValidation { get; }
        public ISpecification<T> Specification { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return KeyValidation;
        }
    }
}

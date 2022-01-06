using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Patterns.Test.Support.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Patterns.Test.Support.Specifications
{
    public class CustomerHasContactSpec : ISpecificationQuery<Customer>
    {
        public Expression<Func<Customer, bool>> IsSatisfiedByExpression => x => x.Contacts.Any();
    }
}

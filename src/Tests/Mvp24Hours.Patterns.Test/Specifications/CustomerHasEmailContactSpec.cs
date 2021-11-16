﻿using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Patterns.Test.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Patterns.Test.Specifications
{
    public class CustomerHasEmailContactSpec : ISpecificationQuery<Customer>
    {
        public Expression<Func<Customer, bool>> IsSatisfiedByExpression => x => x.Contacts.Any(y => y.Type == Enums.ContactType.Email);
    }
}

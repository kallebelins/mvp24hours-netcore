# Specification Pattern
The specification pattern we use as requirements for search filters. Each specification must be created with the objective of defining concrete cases. Each specification may form part of a composition or be applied individually.

> In computer programming, the specification pattern is a particular software design pattern, whereby business rules can be recombined by chaining the business rules together using boolean logic. The pattern is frequently used in the context of domain-driven design. [Wikipedia](https://en.wikipedia.org/wiki/Specification_pattern)

### Example
In this example we create a specification that filters people who have a mobile number in the contact record.

```csharp
/// CustomerHasCellContactSpec.cs
public class CustomerHasCellContactSpec : ISpecificationQuery<Customer>
{
    public Expression<Func<Customer, bool>> IsSatisfiedByExpression => x => x.Contacts.Any(y => y.Type == ContactType.CellPhone);
}

/// CustomerService.cs -> Get Method
Expression<Func<Customer, bool>> filter = x => x.Active;
filter = filter.And<Customer, CustomerHasCellContactSpec>();
var paging = new PagingCriteriaExpression<Customer>(3, 0);
paging.NavigationExpr.Add(x => x.Contacts);
var boResult = service.GetBy(filter, paging);

```

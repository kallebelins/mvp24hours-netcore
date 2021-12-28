# Specification Pattern
O padrão de especificação usamos para aplicar filtros de pesquisa. Cada especificação deve ser criada com o objetivo de definir casos concretos. Cada especificação poderá fazer parte de uma composição ou ser aplicada individualmente.

### Exemplo
Neste exemplo criamos um especificação que filtra pessoas que tenham número de celular no registro de contato.

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

# How to use a service repository?
You will be able to reduce the complexity of building services in the application layer using patterns from the Mvp24Hours architecture.

## Prerequisites
Perform installation and configuration to use a [relational](en-us/database/relational.md) or [NoSQL](en-us/database/nosql.md) database.

## Basic Repository Service
### Inheritance
```csharp
public class EntityService : RepositoryService<Entity, IUnitOfWork> { ... }
```

### Predefined Methods
We use the BusinessService / BusinessObject pattern to encapsulate the response data in the application layer. The methods are:
```csharp
IBusinessResult<bool> ListAny();
IBusinessResult<int> ListCount();
IBusinessResult<IList<TEntity>> List();
IBusinessResult<IList<TEntity>> List(IPagingCriteria criteria);
IBusinessResult<bool> GetByAny(Expression<Func<TEntity, bool>> clause);
IBusinessResult<int> GetByCount(Expression<Func<TEntity, bool>> clause);
IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause);
IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
IBusinessResult<TEntity> GetById(object id);
IBusinessResult<TEntity> GetById(object id, IPagingCriteria criteria);
```

### User-Defined Methods
In this example we will use the referral to get customer contacts. Up to two contacts will be uploaded for each customer, see:
```csharp
public IList<Customer> GetWithContacts()
{
    // create paging for client
    var paging = new PagingCriteria(3, 0);

    // get client repository instance
    var rpCustomer = UnitOfWork.GetRepository<Customer>();

    // apply filter for customers who have contacts with paging
    var customers = rpCustomer.GetBy(x => x.Contacts.Any(), paging);

    // scrolls through customer results to load contacts (late load with filter and/or paging)
    foreach (var customer in customers)
    {
        rpCustomer.LoadRelation(customer, x => x.Contacts, clause: c => c.Active, limit: 2);
    }
    return customers;
}
```

## Basic Services
You can reference the following services as a basis for specialization (Mvp24Hours.Application.Logic):
* RepositoryService: query and commands;
* RepositoryPagingService: query, paged query and commands.
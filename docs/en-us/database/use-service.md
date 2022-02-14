# How to use a service repository?
You will be able to reduce the complexity of building services in the application layer using patterns from the Mvp24Hours architecture.

## Prerequisites
Perform installation and configuration to use a [relational](en-us/database/relational.md) or [NoSQL](en-us/database/nosql.md) database.

## Basic Repository Service
### Inheritance
```csharp
public class EntityService : RepositoryService<Entity, IUnitOfWork> { ... } // async => RepositoryServiceAsync<Entity, IUnitOfWorkAsync>
```

### Predefined Methods
We use the BusinessService / BusinessObject pattern to encapsulate the response data in the application layer. The methods are:
```csharp
IBusinessResult<bool> ListAny(); // async => Task<IBusinessResult<bool>> ListAnyAsync
IBusinessResult<int> ListCount(); // async => Task<IBusinessResult<int>> ListCountAsync
IBusinessResult<IList<TEntity>> List(); // async => Task<IBusinessResult<IList<TEntity>>> ListAsync
IBusinessResult<IList<TEntity>> List(IPagingCriteria criteria); // async => Task<IBusinessResult<IList<TEntity>>> ListAsync
IBusinessResult<bool> GetByAny(Expression<Func<TEntity, bool>> clause); // async => Task<IBusinessResult<bool>> GetByAnyAsync
IBusinessResult<int> GetByCount(Expression<Func<TEntity, bool>> clause); // async => Task<IBusinessResult<int>> GetByCountAsync
IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause); // async => Task<IBusinessResult<IList<TEntity>>> GetByAsync
IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria); // async => Task<IBusinessResult<IList<TEntity>>> GetByAsync
IBusinessResult<TEntity> GetById(object id); // async => Task<IBusinessResult<TEntity>> GetByIdAsync
IBusinessResult<TEntity> GetById(object id, IPagingCriteria criteria); // async => Task<IBusinessResult<TEntity>> GetByIdAsync

// For paged results use:
IPagingResult<IList<TEntity>> ListWithPagination(IPagingCriteria criteria = null); // async => Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync
IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null); // async => Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync
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
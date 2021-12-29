# How to use a Unit of Work?
We use the unit of work pattern to track transactions. According to Martin Fowler:
> Maintains a list of objects affected by a business transaction and coordinates recording of changes and resolving concurrency issues. [Unit Of Work](http://martinfowler.com/eaaCatalog/unitOfWork.html)

## Prerequisites
Perform installation and configuration to use a [relacional](en-us/database/relational.md) or [NoSQL](en-us/database/nosql.md) database.

# Unit of Work
To obtain this, just apply the injection concept through the constructor or use the Mvp24Hours architecture help provider, like this:
```csharp
IUnitOfWork unitOfWork = ServiceProviderHelper.GetService<IUnitOfWork>();
```

## Predefined Methods
```csharp
// IUnitOfWork
int SaveChanges(CancellationToken cancellationToken = default);
void Rollback(CancellationToken cancellationToken = default);
IRepository<T> GetRepository<T>() where T : class, IEntityBase;

// ISQL - Mvp24Hours.Infrastructure.Data.EFCore.SQLServer
IList<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class;
int ExecuteCommand(string sqlCommand, params object[] parameters);
```

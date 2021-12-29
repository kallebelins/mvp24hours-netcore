# How to use a repository?
We use the repository pattern for database interaction. According to Martin Fowler:
> Mediates between the domain and data mapping layers using a collection-like interface to access domain objects. [Repository](http://martinfowler.com/eaaCatalog/repository.html)

## Prerequisites
Perform installation and configuration to use a [relational](en-us/database/relational.md) or [NoSQL](en-us/database/nosql.md) database.

# Repository
Use unit of work to load the repository, like this:
```csharp
IRepository<Entity> rpEntity = UnitOfWork.GetRepository<Entity>();
```

## Predefined Methods
```csharp
// IQuery
bool ListAny();
int ListCount();
IList<TEntity> List();
IList<TEntity> List(IPagingCriteria criteria);
bool GetByAny(Expression<Func<TEntity, bool>> clause);
int GetByCount(Expression<Func<TEntity, bool>> clause);
IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause);
IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
TEntity GetById(object id);
TEntity GetById(object id, IPagingCriteria criteria);

// ICommand
void Add(TEntity entity);
void Add(IList<TEntity> entities);
void Modify(TEntity entity);
void Modify(IList<TEntity> entities);
void Remove(TEntity entity);
void Remove(IList<TEntity> entities);
void RemoveById(object id);
void RemoveById(IList<object> ids);

// IRelation
void LoadRelation<TProperty>(TEntity entity,
	Expression<Func<TEntity, TProperty>> propertyExpression)
	where TProperty : class;
void LoadRelation<TProperty>(TEntity entity,
	Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
	Expression<Func<TProperty, bool>> clause = null,
	int limit = 0)
	where TProperty : class;
void LoadRelationSortByAscending<TProperty, TKey>(TEntity entity,
	Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
	Expression<Func<TProperty, TKey>> orderKey,
	Expression<Func<TProperty, bool>> clause = null,
	int limit = 0)
	where TProperty : class;
void LoadRelationSortByDescending<TProperty, TKey>(TEntity entity,
	Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
	Expression<Func<TProperty, TKey>> orderKey,
	Expression<Func<TProperty, bool>> clause = null,
	int limit = 0)
	where TProperty : class;
```

## Usage Example
```csharp
IRepository<Entity> rpEntity = UnitOfWork.GetRepository<Entity>();

// list all
var entities = rpEntity.List();

// list all with paging
var paging = new PagingCriteria(3, 0); //  limit, offset
var entities = rpEntity.List(paging);

// apply filter
var entities = rpEntity.GetBy(x => x.PropertyName == null);

// apply filter with pagination
var paging = new PagingCriteria(3, 0); //  limit, offset
var entities = rpEntity.GetBy(x => x.PropertyName == null, paging);

// load navigation/relationships
var paging = new PagingCriteria(1, 0, navigation: new List<string> { "PropertyName" });
var entities = rpEntity.List(paging);

// load navigation/relationships with expression
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.NavigationExpr.Add(x => x.PropertyName);
var entities = rpEntity.List(paging);

// apply ordering
var paging = new PagingCriteria(3, 0, orderBy: new List<string> { "PropertyName desc" });
var entities = rpEntity.List(paging);

// apply sorting with expression
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.OrderByDescendingExpr.Add(x => x.PropertyName);
var entities = rpEntity.List(paging);

// load list navigation/relationships with filter and/or paging
var paging = new PagingCriteria(1, 0, navigation: new List<string> { "PropertyList" });
var entities = rpEntity.List(paging);
foreach (var entity in entities)
{
	rpEntity.LoadRelation(entity, x => x.PropertyList, clause: c => c.Active, limit: 1);
}

// load list navigation/relationships with expression with filter and/or pagination
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.NavigationExpr.Add(x => x.PropertyList);
var entities = rpEntity.List(paging);
foreach (var entity in entities)
{
	rpEntity.LoadRelation(entity, x => x.PropertyList, clause: c => c.Active, limit: 1);
}
```
# Como utilizar um repositório?
Utilizamos o padrão de repositório para operações e unidade de trabalho para controlar as transações.

## Pré-Requisitos
Realizar instalação e configuração da [biblioteca](pt-br/database/getting-started.md).

# Unidade de Trabalho
Para obter basta aplicar o conceito de injeção através do construtor ou utilizar o provedor de ajuda da arquitetura Mvp24Hours, assim:
```csharp
IUnitOfWork unitOfWork = ServiceProviderHelper.GetService<IUnitOfWork>();
```

## Métodos Pré-Definidos
```csharp
// IUnitOfWork
int SaveChanges(CancellationToken cancellationToken = default);
void Rollback(CancellationToken cancellationToken = default);
IRepository<T> GetRepository<T>() where T : class, IEntityBase;

// ISQL - SQLServer
IList<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class;
int ExecuteCommand(string sqlCommand, params object[] parameters);
```

# Repositório
Utilize a unidade de trabalho para carregar o repositório, assim:
```csharp
IRepository<Entity> rpEntity = UnitOfWork.GetRepository<Entity>();
```

## Métodos Pré-Definidos
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

## Exemplo de Uso
```csharp
IRepository<Entity> rpEntity = UnitOfWork.GetRepository<Entity>();

// listar tudo
var entities = rpEntity.List();

// listar tudo com paginação
var paging = new PagingCriteria(3, 0); //  limit, offset
var entities = rpEntity.List(paging);

// aplicar filtro
var entities = rpEntity.GetBy(x => x.PropertyName == null);

// aplicar filtro com paginação
var paging = new PagingCriteria(3, 0); //  limit, offset
var entities = rpEntity.GetBy(x => x.PropertyName == null, paging);

// carregar navegação/relacionamentos
var paging = new PagingCriteria(1, 0, navigation: new List<string> { "PropertyName" });
var entities = rpEntity.List(paging);

// carregar navegação/relacionamentos com expressão
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.NavigationExpr.Add(x => x.PropertyName);
var entities = rpEntity.List(paging);

// aplicar ordenação
var paging = new PagingCriteria(3, 0, orderBy: new List<string> { "PropertyName desc" });
var entities = rpEntity.List(paging);

// aplicar ordenação com expressão
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.OrderByDescendingExpr.Add(x => x.PropertyName);
var entities = rpEntity.List(paging);

// carregar navegação/relacionamentos de lista com filtro e/ou paginação
var paging = new PagingCriteria(1, 0, navigation: new List<string> { "PropertyList" });
var entities = rpEntity.List(paging);
foreach (var entity in entities)
{
	rpEntity.LoadRelation(entity, x => x.PropertyList, clause: c => c.Active, limit: 1);
}

// carregar navegação/relacionamentos de lista com expressão com filtro e/ou paginação
var paging = new PagingCriteriaExpression<Entity>(3, 0); //  limit, offset
paging.NavigationExpr.Add(x => x.PropertyList);
var entities = rpEntity.List(paging);
foreach (var entity in entities)
{
	rpEntity.LoadRelation(entity, x => x.PropertyList, clause: c => c.Active, limit: 1);
}
```
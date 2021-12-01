# Como utilizar um repositório de serviço?
Você poderá diminuir a complexidade de construção de serviços na camada de aplicação usando padrões da arquitetura Mvp24Hours.

## Pré-Requisitos
Realizar instalação e configuração da [biblioteca](pt-br/database/getting-started.md).

## Serviço de Repositório Básico
### Herança
```csharp
public class EntityService : RepositoryService<Entity, IUnitOfWork> { ... }
```

### Métodos Pré-Definidos
Utilizamos o padrão de BusinessService / BusinessObject para encapsular os dados de resposta na camada de aplicação. Os métodos são:
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

### Métodos Definidos pelo Usuário
Neste exemplo usaremos a referência para obter contatos de cliente. Serão carregados apenas 2 contatos de cada cliente, veja:
```csharp
public IList<Customer> GetWithContacts()
{
    var paging = new PagingCriteria(2, 0);

    var rpCustomer = UnitOfWork.GetRepository<Customer>();

    var customers = rpCustomer.GetBy(x => x.Contacts.Any(), paging);

    foreach (var customer in customers)
    {
        rpCustomer.LoadRelation(customer, x => x.Contacts);
    }
    return customers;
}
```
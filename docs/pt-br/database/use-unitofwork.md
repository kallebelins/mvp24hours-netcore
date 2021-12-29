# Como utilizar um unidade de trabalho?
Utilizamos o padrão de unidade de trabalho para controlar as transações. Segundo Martin Fowler:
> Mantém uma lista de objetos afetados por uma transação de negócios e coordena a gravação das alterações e a resolução de problemas de simultaneidade. [Unit Of Work](http://martinfowler.com/eaaCatalog/unitOfWork.html)

## Pré-Requisitos
Realizar instalação e configuração para usar um banco de dados [relacional](pt-br/database/getting-started.md) ou [NoSQL](pt-br/database/nosql.md).

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

// ISQL - Mvp24Hours.Infrastructure.Data.EFCore.SQLServer
IList<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class;
int ExecuteCommand(string sqlCommand, params object[] parameters);
```

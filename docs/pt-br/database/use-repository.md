# Como utilizar um repositório?
Utilizamos o padrão de repositório para operações e unidade de trabalho para controlar as transações.

## Pré-Requisitos
Realizar instalação e configuração da [biblioteca](pt-br/database/getting-started.md).

# Unidade de Trabalho
Para obter basta aplicar o conceito de injeção através do construtor ou utilizar o provedor de ajuda da arquitetura Mvp24Hours, assim:
```csharp
IUnitOfWork unitOfWork = ServiceProviderHelper.GetService<IUnitOfWork>();
```

# Repositório
Utilize a unidade de trabalho para carregar o repositório, assim:
```csharp
IRepository<Customer> rpCustomer = UnitOfWork.GetRepository<Customer>();

// listar tudo
var paging = new PagingCriteria(3, 0);
var customers = rpCustomer.List(paging);

// ou 

// obter clientes que possuam contato
var paging = new PagingCriteria(3, 0);
var customers = rpCustomer.GetBy(x => x.Contacts.Any(), paging);

```

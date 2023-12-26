# Mapeamento
> Um mapeador de dados é uma camada de acesso a dados que executa a transferência bidirecional de dados entre um armazenamento de dados persistente (geralmente um banco de dados relacional) e uma representação de dados na memória (a camada de domínio).  [Wikipédia](https://en.wikipedia.org/wiki/Data_mapper_pattern)

## AutoMapper

### Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure -Version 3.12.261
```

### Configuração
```csharp
/// Startup.cs
services.AddMvp24HoursMapService(Assembly.GetExecutingAssembly());
```

### Exemplo de uso
```csharp
/// Customer.cs
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
}

/// CustomerResponse.cs
public class CustomerResponse : IMapFrom
{
    public string Name { get; set; }
    public void Mapping(Profile profile) => profile.CreateMap<Customer, CustomerResponse>();
}

/// ApplicationClass.cs => Method
var customer = new Customer();
// usando extension
var customerResp1 = customer.MapTo<CustomerResponse>();
// usando helper
var customerResp2 = AutoMapperHelper.Map<CustomerResponse>(customer);
```

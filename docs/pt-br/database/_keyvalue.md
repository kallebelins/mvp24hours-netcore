# Banco de Dados Chave-Valor
>Um banco de dados de chave-valor, ou armazenamento de chave-valor, é um paradigma de armazenamento de dados projetado para armazenar, recuperar e gerenciar matrizes associativas e uma estrutura de dados mais comumente conhecida hoje como dicionário ou tabela hash. [Wikipédia](https://pt.wikipedia.org/wiki/Banco_de_dados_de_chave-valor)

## Redis
Estrutura de dados em memória, usado como um banco de dados distribuído de chave-valor, cache e agente de mensagens.

## Pré-Requisitos (Não Obrigatório)
Adicione um arquivo de configuração ao projeto com nome "appsettings.json", conforme abaixo:
```json
{
  "ConnectionStrings": {
    "RedisDbContext": null
  },
  "Mvp24Hours": {
    "Persistence": {
      "Redis": {
        "Enable": true,
        "DefaultExpiration": null,
        "Hosts": [ "localhost:6379" ],
        "InstanceName": null,
        "DefaultDatabase": 0,
        "AbortOnConnectFail": false,
        "AllowAdmin": true,
        "Ssl": false,
        "ConnectTimeout": 6000,
        "ConnectRetry": 2
      }
    }
  }
}

```
Você poderá usar configuração estrutural ou string de conexão.

### Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Data.Caching.Redis
```

### Configuração
```csharp
/// Startup.cs

// estrutural
services.AddMvp24HoursRedisCache();

// string de conexão
services.AddMvp24HoursRedisCache("ConnectionString");

```

### Exemplo de Uso
Você poderá usar o Redis para registrar valor simples ou objetos complexos, assim:


```csharp

// objeto de referência
var customer = new Customer
{
    Oid = Guid.NewGuid(),
    Created = DateTime.Now,
    Name = "Test 1",
    Active = true
};

// adicionar valor simples
string content = customer.ToSerialize();
CacheHelper.SetString("key", content);

// recuperar valor simples
string content = CacheHelper.GetString("key");

// remover valor simples
CacheHelper.RemoveString("key");

// adicionar valor complexo
ObjectCacheHelper.SetObject("key", customer);

// recuperar valor complexo
var customer = ObjectCacheHelper.GetObject<Customer>("key");

// remover valor complexo
CacheHelper.RemoveString("key");

```

Você poderá usar extensions para interagir com a interface IDistributedCache no namespace "Mvp24Hours.Infrastructure.Extensions".

Você ainda poderá usar o conceito de repositório para restringir os tipos exclusivos para uso.

```csharp

/// Startup.cs
services.AddScoped<IRepositoryCache<Customer>, RepositoryCache<Customer>>();

// objeto de referência
var customer = new Customer
{
    Oid = Guid.NewGuid(),
    Created = DateTime.Now,
    Name = "Test 1",
    Active = true
};

// adicionar no formato texto
string content = customer.ToSerialize();
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.SetString("key", content);

// recuperar no formato texto
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
string content = repo.GetString("key");

// remover
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.Remove(_keyString);

// adicionar valor complexo
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.Set("key", customer);

// recuperar valor complexo
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
var customer = repo.Get("key");

```

### Usando Docker
```
// Command
docker run -d -p 6379:6379 -i -t redis:3.2.5-alpine

// Connect
127.0.0.1:6379

```

# Key-Value Database
>A key–value database, or key–value store, is a data storage paradigm designed for storing, retrieving, and managing associative arrays, and a data structure more commonly known today as a dictionary or hash table. [Wikipedia](https://en.wikipedia.org/wiki/Key%E2%80%93value_database)

## Redis
In-memory data structure, used as a distributed key-value database, cache and message agent.

## Prerequisites (Not Required)
Add a configuration file to the project named "appsettings.json", as below:
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
You can use structural configuration or connection string.

### Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Caching.Redis
```

### Configuration
```csharp
/// Startup.cs

// structural
services.AddMvp24HoursRedisCache();

// connection string
services.AddMvp24HoursRedisCache("ConnectionString");

```

### Usage Example
You can use Redis to record simple values or complex objects, like this:

```csharp

// reference object
var customer = new Customer
{
    Oid = Guid.NewGuid(),
    Created = DateTime.Now,
    Name = "Test 1",
    Active = true
};

// add simple value
string content = customer.ToSerialize();
CacheHelper.SetString("key", content);

// retrieve simple value
string content = CacheHelper.GetString("key");

// remove simple value
CacheHelper.RemoveString("key");

// add complex value
ObjectCacheHelper.SetObject("key", customer);

// recover complex value
var customer = ObjectCacheHelper.GetObject<Customer>("key");

// remove complex value
CacheHelper.RemoveString("key");

```

You can use extensions to interact with the IDistributedCache interface in the "Mvp24Hours.Infrastructure.Extensions" namespace.

You can still use the repository concept to restrict the unique types for use.

```csharp

/// Startup.cs
services.AddScoped<IRepositoryCache<Customer>, RepositoryCache<Customer>>();

// reference object
var customer = new Customer
{
    Oid = Guid.NewGuid(),
    Created = DateTime.Now,
    Name = "Test 1",
    Active = true
};

// add in text format
string content = customer.ToSerialize();
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.SetString("key", content);

// retrieve in text format
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
string content = repo.GetString("key");

// remove
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.Remove(_keyString);

// add complex value
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
repo.Set("key", customer);

// recover complex value
var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
var customer = repo.Get("key");

```

### Using Docker
```
// Command
docker run -d -p 6379:6379 -i -t redis:3.2.5-alpine

// Connect
127.0.0.1:6379

```

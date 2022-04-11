# Mapping
> A data mapper is a data access layer that performs the bidirectional transfer of data between a persistent data store (usually a relational database) and an in-memory data representation (the domain layer). [Wikipedia](https://en.wikipedia.org/wiki/Data_mapper_pattern)

## AutoMapper

### Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure -Version 3.4.111
```

### Configuration
```csharp
/// Startup.cs
services.AddMvp24HoursMapService(Assembly.GetExecutingAssembly());
```

### Usage Example
```csharp
/// Customer.cs
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
}

/// CustomerResponse.cs
public class CustomerResponse : IMapFrom<Customer>
{
    public string Name { get; set; }
    public void Mapping(Profile profile) => profile.CreateMap<Customer, CustomerResponse>();
}

/// ApplicationClass.cs => Method
var customer = new Customer();
// using extension
var customerResp1 = customer.MapTo<CustomerResponse>();
// using helper
var customerResp2 = AutoMapperHelper.Map<CustomerResponse>(customer);
```

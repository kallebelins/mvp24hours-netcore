# Data Validation
We can use two methods for data validation, using Fluent Validation or Data Annotations.
Validation is only applied at the time of persisting the data.

## Fluent Validation

### Installation
```csharp
/// Package Manager Console >
Install-Package FluentValidation -Version 10.3.5
```
### Configuration

```csharp
// CustomerValidator.cs
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer {PropertyName} is required.");
    }
}

/// Startup.cs
services.AddSingleton<IValidator<Customer>, CustomerValidator>();
```

## Data Annotations

### Configuration
```csharp
/// Customer.cs

// using
using System.ComponentModel.DataAnnotations;

// implementing
public class Customer : EntityBase<Customer, int>, IEntityBase
{
    public Customer()
    {
        Contacts = new List<Contact>();
    }

    [Required] // annotation
    public string Name { get; set; }

    [Required] // annotation
    public bool Active { get; set; }

    // collections

    public ICollection<Contact> Contacts { get; set; }
}

```

## Notification Context
You can capture validation messages from the notification context. Validations are performed when we try to add or change an entity from the repository. If you do not have an IValidator (FluentValidation) registered (IoC), we assume validations based on annotations (DataAnnotations).

```csharp

// try to create a client, where name is required
var service = ServiceProviderHelper.GetService<CustomerService>();
var customer = new Customer
{
    Active = true
};
service.Add(customer); // try to add entity

// notification pattern
var notfCtxOut = ServiceProviderHelper.GetService<INotificationContext>();
if (notfCtxOut.HasErrorNotifications)
{
    foreach (var item in notfCtxOut.Notifications)
    {
        // Trace.WriteLine(item.Message);
    }
}
```
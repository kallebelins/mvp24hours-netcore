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

## Usage example

```csharp
// apply data validation to the model/entity with FluentValidation or DataAnnotation
var errors = entity.TryValidate(Validator);
if (errors.AnySafe())
{
    return errors.ToBusiness<int>();
}

// perform create action on the database

```
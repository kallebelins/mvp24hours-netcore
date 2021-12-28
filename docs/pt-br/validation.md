# Validação de Dados
Podemos usar dois métodos para validação de dados, usando Fluent Validation ou Data Annotations.
A validação é aplicada apenas no momento de persistir os dados.

## Fluent Validation

### Instalação
```csharp
/// Package Manager Console >

Install-Package FluentValidation -Version 10.3.5
```
### Configuração

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

### Configuração
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

## Contexto de Notificação
Você poderá capturar as mensagens de validação a partir do contexto de notificação. As validações são realizadas no momento em que tentamos adicionar ou alterar uma entidade a partir do repositório. Caso não tenha um IValidator (FluentValidation) registrado (IoC), assumimos validações baseadas em anotações (DataAnnotations).

```csharp

// tenta criar um cliente, onde nome é obrigatório
var service = ServiceProviderHelper.GetService<CustomerService>();
var customer = new Customer
{
    Active = true
};
service.Add(customer); // tenta adicionar entidade

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
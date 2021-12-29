# Notificação (Notification Pattern)
> Um objeto que coleta informações sobre erros e outras informações na camada de domínio e as comunica à apresentação. [Martin Fowler](https://en.wikipedia.org/wiki/Specification_pattern)

### Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure
```

### Configuração
```csharp
/// Startup.cs
services.AddMvp24HoursNotification();
```

### Exemplo de uso
```csharp
var notify = ServiceProviderHelper.GetService<INotificationContext>();

// básico
notify.Add("Test", "Message", Core.Enums.MessageType.Error); // MessageType: Success, Info, Warning, Error

// condicional 
notify.AddIfTrue(1 == 1, "Test", "Message", Core.Enums.MessageType.Error); // MessageType: Success, Info, Warning, Error
```

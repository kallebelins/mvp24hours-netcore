# Notification Pattern
> An object that collects information about errors and other information at the domain layer and communicates it to the presentation. [Martin Fowler](https://en.wikipedia.org/wiki/Specification_pattern)

### Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure
```

### Configuration
```csharp
/// Startup.cs
services.AddMvp24HoursNotification();
```

### Usage Example
```csharp
var notify = ServiceProviderHelper.GetService<INotificationContext>();

// basic
notify.Add("Test", "Message", Core.Enums.MessageType.Error); // MessageType: Success, Info, Warning, Error

// conditional 
notify.AddIfTrue(1 == 1, "Test", "Message", Core.Enums.MessageType.Error); // MessageType: Success, Info, Warning, Error
```

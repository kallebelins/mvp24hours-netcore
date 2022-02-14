# Message Broker
> Message Broker is software that allows applications, systems and services to communicate with each other and exchange information. [What is a Message Broker?](https://medium.com/@bookgrahms/o-que-%C3%A9-um-corretor-de-mensagens-message-broker-c9fbe219443b)

## RabbitMQ

### Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.RabbitMQ -Version 3.2.142
```

### Configuration
```csharp
/// Startup.cs

services.AddMvp24HoursRabbitMQ(options =>
{
    options.ConnectionString = Configuration.GetConnectionString("RabbitMQContext"); // amqp://guest:guest@localhost:5672
});

services.AddScoped<CustomerProducer, CustomerProducer>();
services.AddScoped<CustomerConsumer, CustomerConsumer>();

```

### Producer/Consumer Implementation

```csharp
/// CustomerProducer.cs
public class CustomerProducer : MvpRabbitMQProducer<CustomerDto>
{
    public CustomerProducer(IOptions<RabbitMQOptions> options)
        : base(options)
    {
    }
}

/// CustomerService.cs // Save Method
producer.Publish(new CustomerDto
{
    Id = 99,
    Name = "Test 1",
    Active = true
});

/// CustomerConsumer.cs
public class CustomerConsumer : MvpRabbitMQConsumer<CustomerDto>
{
    public CustomerConsumer(IOptions<RabbitMQOptions> options)
        : base(options)
    {
    }
    public override void Received(object message)
    {
        Trace.WriteLine($"Received customer {(message as CustomerDto)?.Name}");
    }
} 

/// HostService.cs
var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
while (!source.IsCancellationRequested)
{
    var consumer = serviceProvider.GetService<CustomerConsumer>();
    consumer.Consume();
}

```

### Using Docker
```
// Command
docker run -d --name my-rabbit -p 5672:5672 -p 5673:5673 -p 15672:15672 rabbitmq:3-management

// Connect
[127.0.0.1:6379](amqp://guest:guest@localhost:5672)

```

# Message Broker
>Corretor de mensagens (ou Message Broker) é um software que permite que aplicações, sistemas e serviços comuniquem entre si e troquem informações. [O que é um Corretor de Mensagens ( Message Broker ) ?](https://medium.com/@bookgrahms/o-que-%C3%A9-um-corretor-de-mensagens-message-broker-c9fbe219443b)

## RabbitMQ

### Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.RabbitMQ -Version 3.2.171
```

### Configuração
```csharp
/// Startup.cs

services.AddMvp24HoursRabbitMQ(options =>
{
    options.ConnectionString = Configuration.GetConnectionString("RabbitMQContext"); // amqp://guest:guest@localhost:5672
});

services.AddScoped<CustomerProducer, CustomerProducer>();
services.AddScoped<CustomerConsumer, CustomerConsumer>();

```

### Implementação de Producer/Consumer

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

### Usando Docker
```
// Command
docker run -d --name my-rabbit -p 5672:5672 -p 5673:5673 -p 15672:15672 rabbitmq:3-management

// Connect
[127.0.0.1:6379](amqp://guest:guest@localhost:5672)

```

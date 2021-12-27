# Message Broker
Em contrução...

## RabbitMQ

### Configuração
```csharp

// Producers

services.AddScoped<CustomerProducer, CustomerProducer>();

// class
public class CustomerProducer : MvpRabbitMQProducer
{
    public CustomerProducer()
        : base(EventBusConstants.CustomerQueue) { }
}

// Consumers
services.AddScoped<CustomerConsumer, CustomerConsumer>();

// class
public class CustomerConsumer : MvpRabbitMQConsumer<CustomerEvent>
{
    public CustomerConsumer()
        : base(EventBusConstants.CustomerQueue)
    {
    }

    public override Task Received(CustomerEvent message)
    {
        Trace.WriteLine($"Received customer {message?.Name}");
        return Task.FromResult(false);
    }
}       

```

### Usando Docker
```
// Command
docker run -d --name my-rabbit -p 5672:5672 -p 5673:5673 -p 15672:15672 rabbitmq:3-management

// Connect
[127.0.0.1:6379](amqp://guest:guest@localhost:5672)

```

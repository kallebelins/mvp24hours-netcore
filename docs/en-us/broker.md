# Message Broker
> Message Broker is software that allows applications, systems and services to communicate with each other and exchange information. [What is a Message Broker?](https://medium.com/@bookgrahms/o-que-%C3%A9-um-corretor-de-mensagens-message-broker-c9fbe219443b)

## RabbitMQ

### Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.RabbitMQ -Version 3.12.262
```


### Basic Configuration
```csharp
/// Startup.cs

services.AddMvp24HoursRabbitMQ(
    typeof(MyClassConsumer).Assembly,
    connectionOptions =>
    {
        connectionOptions.ConnectionString = configuration.GetConnectionString("RabbitMQContext");
        connectionOptions.DispatchConsumersAsync = true;
        connectionOptions.RetryCount = 3;
    }
);

```

### Advanced Configuration
Dead Letter Queue configuration example:

```csharp
/// Startup.cs

services.AddMvp24HoursRabbitMQ(
    typeof(MyClassConsumer).Assembly,
    connectionOptions =>
    {
        connectionOptions.ConnectionString = configuration.GetConnectionString("RabbitMQContext");
        connectionOptions.DispatchConsumersAsync = true;
        connectionOptions.RetryCount = 3;
    },
    clientOptions =>
    {
        clientOptions.Exchange = "customer.direct";
        clientOptions.MaxRedeliveredCount = 1;
        clientOptions.QueueArguments = new System.Collections.Generic.Dictionary<string, object>
        {
            { "x-queue-mode", "lazy" },
            { "x-dead-letter-exchange", "dead-letter-customer.direct" }
        };

        // dead letter exchanges enabled
        clientOptions.DeadLetter = new RabbitMQOptions()
        {
            Exchange = "dead-letter-customer.direct",
            QueueArguments = new System.Collections.Generic.Dictionary<string, object>
            {
                { "x-queue-mode", "lazy" }
            }
        };
    }
);

```

### Producer/Consumer Implementation

```csharp
/// CustomerService.cs // Save Method
var client = serviceProvider.GetService<MvpRabbitMQClient>();
client.Publish(new CustomerDto
{
    Id = 99,
    Name = "Test 1",
    Active = true
}, typeof(CustomerDto).Name);

/// CustomerConsumer.cs
public class CustomerConsumer : IMvpRabbitMQConsumerAsync
{
    public string RoutingKey => typeof(CustomerDto).Name;

    public string QueueName => typeof(CustomerDto).Name;

    public async Task ReceivedAsync(object message, string token)
    {
        // take action
        await Task.CompletedTask;
    }

    public async Task FailureAsync(Exception exception, string token)
    {
        // perform handling for integration failures in RabbitMQ
        // write to a temp table, send email, create specialized log, etc.
        await Task.CompletedTask;
    }

    public async Task RejectedAsync(object message, string token)
    {
        // we tried to consume the resource for 3 times, in this case as we did not treat it, we will disregard
        // write to a temp table, send email, create specialized log, etc.
        await Task.CompletedTask;
    }
}

/// HostService.cs
var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
while (!source.IsCancellationRequested)
{
    var client = serviceProvider.GetService<MvpRabbitMQClient>();
    client.Consume();
}

```

### Using Docker
```
// Command
docker run -d --name my-rabbit -p 5672:5672 -p 5673:5673 -p 15672:15672 rabbitmq:3-management

// Connect
[127.0.0.1:6379](amqp://guest:guest@localhost:5672)

```

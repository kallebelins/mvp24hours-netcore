# Telemetry
Solution created to track all application execution levels. You will be able to inject actions for treatment using any log manager, including metrics and trace.

## Configuration
```csharp

/// Startup.cs
Logger logger = LogManager.GetCurrentClassLogger(); // any log manager

// trace
services.AddMvp24HoursTelemetry(TelemetryLevel.Information | TelemetryLevel.Verbose,
    (name, state) =>
    {
        logger.Trace($"{name}|{string.Join("|", state)}");
    }
);

// error
services.AddMvp24HoursTelemetry(TelemetryLevel.Error,
    (name, state) =>
    {
        if (name.EndsWith("-failure"))
        {
            logger.Error(state.ElementAtOrDefault(0) as Exception);
        }
        else
        {
            logger.Error($"{name}|{string.Join("|", state)}");
        }
    }
);

// ignore events
services.AddMvp24HoursTelemetryIgnore("rabbitmq-consumer-basic");

```

## Run /  Execute
```csharp
/// MyFile.cs
TelemetryHelper.Execute(TelemetryLevel.Verbose, "rabbitmq-client-publish-start", $"token:{tokenDefault}");

```
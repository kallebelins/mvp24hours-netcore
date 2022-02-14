# Pipeline (Pipe and Filters Pattern)
It is a design pattern that represents a tube with several operations (filters), executed sequentially, in order to travel, integrate and/or handle a package/message.

## Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Pipe -Version 3.2.14
```

## Configuration
```csharp
/// Startup.cs

// default
services.AddMvp24HoursPipeline(options => // async => AddMvp24HoursPipelineAsync
{
    options.IsBreakOnFail = false;
});

// with factory
services.AddMvp24HoursPipeline(factory: (x) => // async => AddMvp24HoursPipelineAsync
{
    var pipeline = new Pipeline(x.GetRequiredService<INotificationContext>()); // async => PipelineAsync
    pipeline.AddInterceptors(input =>
    {
        input.AddContent<int>("factory", 1);
        System.Diagnostics.Trace.WriteLine("Interceptor factory.");
    }, Core.Enums.Infrastructure.PipelineInterceptorType.PostOperation);
    return pipeline;
});
```

## Usage Example
```csharp
var pipeline = serviceProvider.GetService<IPipeline>(); // async => IPipelineAsync

// run pipeline
pipeline.Execute(); // async => ExecuteAsync

// run with package/message
var message = "Parameter received.".ToMessage();
pipeline.Execute(message); // async => ExecuteAsync

// get package after run
pipeline.GetMessage();

// add IOperation operation/filter
pipeline.Add<MyOperation>();

// add operation/filter as action
pipeline.Add(_ =>
{
    Trace.WriteLine("Test 1");
});

// package interaction actions by type
pipeline.Add(input =>
{
    string param = input.GetContent<string>(); // get content
    input.AddContent($"Test 1 - {param}"); // add content
    if (input.HasContent<string>()) {} // check for content
    input.SetLock(); // block package/message
    input.SetFailure(); // log failure
});

// packet interaction actions by key
pipeline.Add(input =>
{
    string param = input.GetContent<string>("key"); // get content with key
    input.AddContent("key", $"Test 1 - {param}"); // add content with key
    if (input.HasContent("key")) {} // check for content with key
    input.SetLock(); // block package/message
    input.SetFailure(); // log failure
});

// adding interceptors
pipeline.AddInterceptors(_ =>
{
    // ... commands
}, PipelineInterceptorType.PostOperation); //  PostOperation, PreOperation, Locked, Faulty, FirstOperation, LastOperation

// adding conditional interceptors
pipeline.AddInterceptors(_ =>
{
    // ... commands
},
input =>
{
    return input.HasContent<int>();
});

// adding interceptors as events
pipeline.AddInterceptors((input, e) =>
{
    // ... commands
}, PipelineInterceptorType.PostOperation); //  PostOperation, PreOperation, Locked, Faulty, FirstOperation, LastOperation

// adding conditional interceptors as events
pipeline.AddInterceptors((input, e) =>
{
    // ... commands
},
input =>
{
    return input.HasContent<int>();s
});

```

### Creating Operations
To create an operation, simply implement an IOperation or an OperationBase:

```csharp
/// MyOperation.cs
public class MyOperation : OperationBase // async => OperationBaseAsync
{
    public override bool IsRequired => false; // indicates if the operation will execute even with the locked package

    public override void Execute(IPipelineMessage input) // async => Task ExecuteAsync
    {
        // perform action
        return input;
    }
}

// add to pipeline
pipeline.Add<MyOperation>();
```

### Creating Builders
You can add dynamic operations using a build pattern (builder). Generally, we use it when implementing Ports And Adapters architectures where we plug in adapters that implement specialized rules.

```csharp
/// ..my-core/contract/builders/IProductCategoryListBuilder.cs
public interface IProductCategoryListBuilder : IPipelineBuilder { } // async => IPipelineBuilderAsync

/// ..my-adapter-application/application/builders/ProductCategoryListBuilder.cs
public class ProductCategoryListBuilder : IProductCategoryListBuilder
{
    public IPipeline Builder(IPipeline pipeline) // async => IPipelineAsync
    {
        return pipeline
            .Add<ProductCategoryFileOperation>()
            .Add<ProductCategoryResponseMapperOperation>();
    }
}

/// Startup.cs
services.AddScoped<IProductCategoryListBuilder, ProductCategoryListBuilder>();

/// ..my-application/application/services/MyService.cs /MyMethod
var pipeline = serviceProvider.GetService<IPipeline>(); // async => IPipelineAsync
var builder = serviceProvider.GetService<IProductCategoryListBuilder>();
builder.Builder(pipeline);
```
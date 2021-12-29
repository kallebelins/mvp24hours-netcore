# Pipeline (Pipe and Filters Pattern)
It is a design pattern that represents a tube with several operations (filters), executed sequentially, in order to travel, integrate and/or handle a package/message.

## Prerequisites (Not Required)
Add a configuration file to the project named "appsettings.json", as below:
```json
{
  "Mvp24Hours": {
    "Pipe": {
      "Operation": {
        "FileLog": {
          "Enable": false,
          "Path": ""
        },
        "FileToken": {
          "Enable": false,
          "Path": ""
        }        
      }
    }
  }
}
```

## Installation
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Pipe
```

## Configuration
```csharp
/// Startup.cs
services.AddMvp24HoursPipeline();
```

## Usage Example
```csharp
var pipeline = ServiceProviderHelper.GetService<IPipeline>();

// run pipeline
pipeline.Execute();

// run with package/message
var message = "Parameter received.".ToMessage();
pipeline.Execute(message);

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
public class MyOperation : OperationBase
{
    public override bool IsRequired => false; // indicates if the operation will execute even with the locked package

    public override IPipelineMessage Execute(IPipelineMessage input)
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
public interface IProductCategoryListBuilder : IPipelineBuilder { }

/// ..my-adapter-application/application/builders/ProductCategoryListBuilder.cs
public class ProductCategoryListBuilder : IProductCategoryListBuilder
{
    public IPipelineAsync Builder(IPipelineAsync pipeline)
    {
        return pipeline
            .Add<ProductCategoryFileOperation>()
            .Add<ProductCategoryResponseMapperOperation>();
    }
}

/// Startup.cs
services.AddScoped<IProductCategoryListBuilder, ProductCategoryListBuilder>();

/// ..my-application/application/services/MyService.cs /MyMethod
var pipeline = ServiceProviderHelper.GetService<IPipeline>();
var builder = ServiceProviderHelper.GetService<IProductCategoryListBuilder>();
builder.Builder(pipeline);
```
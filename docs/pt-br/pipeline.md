# Pipeline (Pipe and Filters Pattern)
É um padrão de projeto que representa um tubo com diversas operações (filtros), executadas de forma sequencial, com o intuito de trafegar, integir e/ou manuear um pacote/mensagem.

## Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Pipe -Version 3.2.151
```

## Configuração
```csharp
/// Startup.cs

// padrão
services.AddMvp24HoursPipeline(options => // async => AddMvp24HoursPipelineAsync
{
    options.IsBreakOnFail = false;
});

// com factory
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

## Exemplo de Uso
```csharp
var pipeline = serviceProvider.GetService<IPipeline>(); // async => IPipelineAsync

// executar pipeline
pipeline.Execute(); // async => ExecuteAsync

// executar com pacote/mensagem
var message = "Parameter received.".ToMessage();
pipeline.Execute(message); // async => ExecuteAsync

// obter pacote após execução
pipeline.GetMessage();

// adicionar operação/filtro de IOperation
pipeline.Add<MyOperation>();

// adicionar operação/filtro como action
pipeline.Add(_ =>
{
    Trace.WriteLine("Test 1");
});

// ações de interação com pacote por tipo
pipeline.Add(input =>
{
    string param = input.GetContent<string>(); // obter conteúdo
    input.AddContent($"Test 1 - {param}"); // adicionar conteúdo
    if (input.HasContent<string>()) {} // verifica se tem conteúdo
    input.SetLock(); // bloquear pacote/mensagem
    input.SetFailure(); // registrar falha
});

// ações de interação com pacote por chave
pipeline.Add(input =>
{
    string param = input.GetContent<string>("key"); // obter conteúdo com chave
    input.AddContent("key", $"Test 1 - {param}"); // adicionar conteúdo com chave
    if (input.HasContent("key")) {} // verifica se tem conteúdo com chave
    input.SetLock(); // bloquear pacote/mensagem
    input.SetFailure(); // registrar falha
});

// adicionando interceptadores
pipeline.AddInterceptors(_ =>
{
    // ... comandos
}, PipelineInterceptorType.PostOperation); //  PostOperation, PreOperation, Locked, Faulty, FirstOperation, LastOperation

// adicionando interceptadores condicionais
pipeline.AddInterceptors(_ =>
{
    // ... comandos
},
input =>
{
    return input.HasContent<int>();
});

// adicionando interceptadores como eventos
pipeline.AddInterceptors((input, e) =>
{
    // ... comandos
}, PipelineInterceptorType.PostOperation); //  PostOperation, PreOperation, Locked, Faulty, FirstOperation, LastOperation

// adicionando interceptadores condicionais como eventos
pipeline.AddInterceptors((input, e) =>
{
    // ... comandos
},
input =>
{
    return input.HasContent<int>();s
});

```

### Criando Operações
Para criar uma operação basta implementar uma IOperation ou uma OperationBase:

```csharp
/// MyOperation.cs
public class MyOperation : OperationBase // async => OperationBaseAsync
{
    public override bool IsRequired => false; // indica se a operação irá executar mesmo com o pacote bloqueado

    public override void Execute(IPipelineMessage input) // async => Task ExecuteAsync
    {
        // executa ação
        return input;
    }
}

// adicionar ao pipeline
pipeline.Add<MyOperation>();
```

### Criando Construtores
Você poderá adicionar operações dinâmicas usando um padrão de construção (builder). Geralmente, usamos ao implementar arquiteturas Ports And Adapters onde encaixamos adaptadores que implementam regras especializadas.

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
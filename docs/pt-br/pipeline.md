# Pipeline
É um padrão de projeto que representa um tubo com diversas operações (filtros), executadas de forma sequencial, com o intuito de trafegar, integir e/ou manuear um pacote/mensagem.

## Pré-Requisitos (Não Obrigatório)
Adicione um arquivo de configuração ao projeto com nome "appsettings.json", conforme abaixo:
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

## Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure.Pipe
```

## Configuração
```csharp
/// Startup.cs

services.AddMvp24HoursPipeline();
```

## Exemplo de Uso
```csharp
var pipeline = ServiceProviderHelper.GetService<IPipeline>();

// executar pipeline
pipeline.Execute();

// executar com pacote/mensagem
var message = "Parameter received.".ToMessage();
pipeline.Execute(message);

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
public class MyOperation : OperationBase
{
    public override bool IsRequired => false; // indica se a operação irá executar mesmo com o pacote bloqueado

    public override IPipelineMessage Execute(IPipelineMessage input)
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
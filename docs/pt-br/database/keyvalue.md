# Banco de Dados Chave-Valor
Em contrução...

## Redis
Estrutura de dados em memória, usado como um banco de dados distribuído de chave-valor, cache e agente de mensagens.

### Configuração
```csharp
/// Startup.cs

services.AddMvp24HoursRedisCache();

```

### Usando Docker
```
// Command
docker run -d -p 6379:6379 -i -t redis:3.2.5-alpine

// Connect
127.0.0.1:6379

```

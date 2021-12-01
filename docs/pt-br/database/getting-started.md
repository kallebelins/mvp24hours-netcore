# Banco de Dados Relacional
Foi implementado padrão de repositório com critérios de pesquisa e paginação, além de unidade de trabalho. Usamos Entity Framework para realizar persistência. O Entity Framework dá suporte a diversos bancos de dados e os que já foram testados são:

## Pré-Requisitos (Não Obrigatório)
Adicione um arquivo de configuração ao projeto com nome "appsettings.json". O arquivo deverá conter um chave com dados de conexão, por exemplo, ConnectionStrings/DataContext conforme abaixo:
```json
{
  "ConnectionStrings": {
    "DataContext": "String de conexão"
  },
  "Mvp24Hours": {
    "Persistence": {
      "MaxQtyByQueryPage": 30,
      "ReadUncommitedQuery": false
    }
  }
}
```
Você poderá usar a conexão de banco de dados direto, o que não é recomendado. Acesse o site [ConnectionStrings](https://www.connectionstrings.com/) e veja como montar a conexão com seu banco.

## PostgreSql
### Instalação
```csharp
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 5.0.10
```
### Configuração
```csharp
/// Startup.cs

services.AddDbContext<DataContext>(
    options => options.UseNpgsql(ConfigurationHelper.AppSettings.GetConnectionString("DataContext"),
    options => options.SetPostgresVersion(new Version(9, 6)))
);

services.AddMvp24HoursDbService<DataContext>();

```

## MySql
### Instalação
```csharp
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package MySql.EntityFrameworkCore -Version 5.0.8
```
### Configuração
```csharp
/// Startup.cs

services.AddDbContext<DataContext>(options =>
    options.UseMySQL(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

services.AddMvp24HoursDbService<DataContext>();

```

## SQL Server
### Instalação
```csharp
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 5.0.10
```
### Configuração
```csharp
/// Startup.cs

services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

services.AddMvp24HoursDbService<DataContext>();

```
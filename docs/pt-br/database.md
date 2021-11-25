# Banco de Dados Relacional
Foi implementado padrão de repositório com critérios de pesquisa e paginação, além de unidade de trabalho. Usamos Entity Framework para realizar persistência. O Entity Framework dá suporte a diversos bancos de dados e os que já foram testados são:

## PostgreSql
### Instalação
```
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 5.0.10
```
### Configuração
```
/// Startup.cs

services.AddDbContext<DataContext>(
    options => options.UseNpgsql(ConfigurationHelper.AppSettings.GetConnectionString("DataContext"),
    options => options.SetPostgresVersion(new Version(9, 6)))
);

services.AddMvp24HoursDbService<DataContext>();

```

## MySql
### Instalação
```
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package MySql.EntityFrameworkCore -Version 5.0.8
```
### Configuração
```
/// Startup.cs

services.AddDbContext<DataContext>(options =>
    options.UseMySQL(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

services.AddMvp24HoursDbService<DataContext>();

```

## SQL Server
### Instalação
```
/// Package Manager Console >

Install-Package Microsoft.Extensions.DependencyInjection -Version 6.0.0
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 5.0.10
```
### Configuração
```
/// Startup.cs

services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

services.AddMvp24HoursDbService<DataContext>();

```
# NoSQL Database
>A NoSQL (originally referring to "non-SQL" or "non-relational")[1] database provides a mechanism for storage and retrieval of data that is modeled in means other than the tabular relations used in relational databases. [Wikipedia](https://en.wikipedia.org/wiki/NoSQL)

Repository pattern was implemented with search and paging criteria, in addition to unit of work ([See Repository](en-us/database/use-repository)). This implementation does not only support late loading of related objects.

## Prerequisites (Not Required)
Add a configuration file to the project named "appsettings.json". The file must contain a key with connection data, for example, ConnectionStrings/DataContext as below:
```json
{
  "ConnectionStrings": {
    "DataContext": "connection string"
  },
  "Mvp24Hours": {
    "Persistence": {
      "MaxQtyByQueryPage": 30
    }
  }
}
```
You will be able to use direct database connection, which is not recommended. Access the [ConnectionStrings](https://www.connectionstrings.com/) website and see how to set up the connection with your bank.

## MongoDB
Additional configuration for MongoDb registered in "appsettings.json":
```json
{
  "Mvp24Hours": {
    "Persistence": {
      "MongoDb": {
        "EnableTls": false,
        "EnableTransaction": false
      }
    }
  }
}
```

### Installation
```csharp
/// Package Manager Console >
Install-Package MongoDB.Driver -Version 2.13.2
Install-Package Mvp24Hours.Infrastructure.Data.MongoDb
```
### Configuration
```csharp
/// Startup.cs

services.AddMvp24HoursMongoDb("mycollection", ConfigurationHelper.GetSettings("ConnectionStrings:DataContext"));
```

### Using Docker
**Basic Command**
```
// Command
docker run -d --name mongo -p 27017:27017 mvertes/alpine-mongo

// ConnectionString
mongodb://localhost:27017

```

**Database Command with Password**
```
// Command
docker run --name mongodb -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=user -e MONGO_INITDB_ROOT_PASSWORD=123456 mongo

// ConnectionString
mongodb://user:123456@localhost:27017

```

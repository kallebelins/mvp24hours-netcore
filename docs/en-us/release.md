# What's new?

## v3.12.262
* Refactoring to migrate json file settings to fluent extensions;
* Replacement of the notification pattern;
* Review of templates;
* Added HealthCheck on all samples;
* Basic WebStatus project creation with HealthCheckUI;
* Replacement of logging dependencies for trace injection through actions;
* Trace/Verbose on all main libraries and layers;
* Transaction isolation level configuration for queries with EF;
* RabbitMQ library refactoring for consumer injection and fluid configuration for "DeadLetterQueue";
* Persistent connection and resiliency with Polly for RabbitMQ;
* Asynchronous consumer implementation for RabbitMQ;
* Pipeline adjustment to allow adding messages in the package (info, error, warning, success) - replacement of the notification pattern;
* Validation change (FluentValidation or DataAnnotations) to return message list - notification pattern replacement;
* Documentation change and configuration addition for WebAPI;
* Test library refactoring;
* Refactoring for migration from Core to .NET 6.

## Other versions...
* Relational database (SQL Server, PostgreSql and MySql)
* NoSql database (MongoDb and Redis)
* Message Broker (RabbitMQ)
* Pipeline (Pipe and Filters pattern)
* Documentation (Swagger)
* Mapping (AutoMapper)
* Logging
* Standards for data validation (FluentValidation and Data Annotations), specifications (Specification pattern), unit of work, repository, among others.
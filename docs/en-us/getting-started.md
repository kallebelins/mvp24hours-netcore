# Getting Started
Each architectural solution must be built based on technical and/or business needs.
The purpose of this library is to ensure agility in the construction of digital products through structures, mechanisms and tools that, correctly combined, offer robustness, security, performance, monitoring, observability, resilience and consistency.
Below are the main references to RESTful API for persistence and service integration.

## Relational database
It is a database that allows you to create relationships with each other in order to ensure consistency and data integrity.

* [SQL Server](en-us/database/relational?id=sql-server)
* [PostgreSql](en-us/database/relational?id=postgresql)
* [MySql](en-us/database/relational?id=mysql)

## NoSql Database
NoSQL is a generic term that represents a non-relational database.

[MongoDb](en-us/database/nosql?id=mongodb)

## Key-Value Database
It is a map or dictionary data structure, where we use a key as the record identifier.

[Redis](en-us/database/keyvalue?id=redis.md)

## Message Broker
A message broker is software that makes it possible for applications, systems and services to communicate and exchange information.

[RabbitMQ](en-us/broker.md)

## Pipeline
It is a design pattern that represents a tube with several operations (filters), executed sequentially, in order to travel, integrate and/or handle a package/message.

[Pipeline](en-us/pipeline.md)

## Documentation
The habit of documenting interfaces and data classes (value objects, dtos, entities, ...) can contribute to facilitate code maintenance. Swagger allows you to easily document your RESTful API by sharing with other developers how they can consume available resources.

[Swagger](en-us/swagger.md)

## Mapping
With the practice of RESTful API development with a focus on mobile, we have as reference to offer as little data as possible or necessary in each API resource. Therefore, there is a need to create specific traffic objects (DTOs).
AutoMapper assists in assigning property values to related objects (Person =>PersonDto), or rather, mapping one object to another.

[AutoMapper](en-us/automapper.md)

## Patterns
Patterns are models that we use as a reference or basis for solving a problem. In addition to the patterns that we will present in the architectural definitions explored by this library, we explore the following references:

* [Unit of Work](en-us/database/use-unitofwork.md)
* [Repository](en-us/database/use-repository.md)
* [Repository Service](en-us/database/use-service.md): We use it to apply business rules and encapsulate the repository
* [Data Validation](en-us/validation.md): Data validation using fluent or annotations
* [Notification](en-us/notification.md): Exchanging messages in a notifications context
* [Specification](en-us/specification.md): Data filter
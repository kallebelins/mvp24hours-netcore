# <img  style="vertical-align:middle" width="42" height="42" src="/_media/icon.png" alt="Mvp24Hours" /> Mvp24Hours (v3.12.151)


This project was developed to contribute to the rapid construction of services. I used the reference of solutions for building microservices.

## Features
* Relational database (SQL Server, PostgreSql and MySql)
* NoSql database (MongoDb and Redis)
* Message Broker (RabbitMQ)
* Pipeline (Pipe and Filters pattern)
* Documentation (Swagger)
* Mapping (AutoMapper)
* Logging
* Standards for data validation (FluentValidation and Data Annotations), specifications (Specification pattern), unit of work, repository, among others.

## Samples
You can study different solutions with the Mvp24Hours library. Also, create quick projects using templates for Visual Studio 2019 and 2022.
<br>Visit: https://github.com/kallebelins/mvp24hours-netcore-samples
<br>Templates: https://github.com/kallebelins/mvp24hours-netcore-samples/tree/main/vstemplate

## Next Steps

~~* Implement request with HttpFactory to apply resilience concepts;~~
* Implement request with Consul (Service Discovery) using service key;
~~* Create http log for monitoring unique resources;~~
* Create project template using Consul (Service Discovery);
~~* Create project model for observability/monitoring application with ElasticSearch (ELK) - distributed log;~~
* Create project model with ASP.Net Identity;
~~* Create project model with dynamic generation of classes with [Mvp24Hours-Entity-T4](https://github.com/kallebelins/mvp24hours-entity-t4);~~
~~* Create design model to apply resilience and fault tolerance concepts;~~
* Create project template with Grpc over HTTP2 (server and client);
* Implement integration with Kafka (message broker);
~~* Create project template with WatchDog to monitor the health of services;~~
* Create project template for gateway (ocelot) with service discovery (consul);
* Create project template for gateway (ocelot) with aggregator;
* Record training videos for the community;

## Donations
Please consider donating if you think this library is useful to you or that my work is valuable. Glad if you can help me [buy a cup of coffee](https://www.paypal.com/donate/?hosted_button_id=EKA2L256GJVQC). :heart:

## Community
Users, stakeholders, students, enthusiasts, developers, programmers [connect on Telegram](https://t.me/+6_sL0y2TE-ZkMmZh) to follow our growth closely!

## Sponsors
Be a sponsor by choosing this project to accelerate your products.

## Release

### 3.12.221
* Implementation of Delegation Handlers to propagate keys in the Header (correlation-id, authorization, etc);
* Implementation of Polly to apply resilience and fault tolerance concepts;
* Correction of automatic loading of mapping classes with IMapFrom;

### 3.12.151
* Removed generic typing from the IMapFrom class;
* Implementation of Testcontainers for RabbitMQ, Redis and MongoDb projects;
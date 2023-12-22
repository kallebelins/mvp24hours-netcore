# O que há de novo?

## v3.12.221
* Refatoração para migrar configurações de arquivo json para extensões fluentes;
* Substituição do padrão de notificação;
* Revisão dos templates;
* Adição de HealthCheck em todos os samples;
* Criação de projeto básico de WebStatus com HealthCheckUI;
* Substituição de depências de logging para injeção de trace através de actions;
* Trace/Verbose em todas as bibliotecas e camadas principais;
* Configuração de nível de isolamento de transação para consultas com EF;
* Refatoração da biblioteca do RabbitMQ para injeção de consumers e configuração fluída para "DeadLetterQueue";
* Conexão persistente e resiliência com Polly para RabbitMQ;
* Implementação de consumidor assíncrono para RabbitMQ;
* Ajuste de pipeline para permitir adicionar mensagens no pacote (info, error, warning, success) - substituição do padrão de notificação;
* Alteração de validação (FluentValidation ou DataAnnotations) para retornar lista de mensagens - substituição do padrão de notificação;
* Alteração de documentação e adição de configuração para WebAPI;
* Refatoração do teste de bibliotecas;
* Refatoração para migração do Core para o .NET 6.

## Outras versões...
* Banco de dados relacional (SQL Server, PostgreSql e MySql)
* Banco de dados NoSql (MongoDb e Redis)
* Message Broker (RabbitMQ)
* Pipeline (Pipe and Filters pattern)
* Documentação (Swagger)
* Mapeamento (AutoMapper)
* Logging
* Padrões para validação de dados (FluentValidation e Data Annotations), especificações (Specification pattern), unidade de trabalho, repositório, entre outros.
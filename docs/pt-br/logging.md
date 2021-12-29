# Logging
> O Log de uma aplicação não pode ser como a narração de um jogo de futebol onde cada lance é registrado. Ele deve conter informações relevantes que podem ser disponibilizadas em diferentes níveis de detalhe determinados pela necessidade da investigação de algum evento ocorrido na aplicação. [DevMedia](https://www.devmedia.com.br/aprenda-a-utilizar-o-log-revista-easy-java-magazine-21/25479)

Em nossa biblioteca usamos NLog pela facilidade de definição e implantação.

## Instalação
```csharp
/// Package Manager Console >
Install-Package Mvp24Hours.Infrastructure
```

## Configuração
```csharp
/// Startup.cs
services.AddMvp24HoursLogging();
```

A configuração pode ser através de arquivo. Basta criar um arquivo "NLog.config" no diretório da aplicação. O conteúdo do arquivo é no formato XML. Veja abaixo.

### Log Console
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	  autoReload="true">
  <targets>
    <target name="console"
            xsi:type="ColoredConsole"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}" />
    <target name="debug"
            xsi:type="Debugger"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="console,debug" />
  </rules>
</nlog>
```

### Log Arquivo
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	  autoReload="true">
  <targets>
    <target name="logfile"
            xsi:type="File"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}"
            fileName="${basedir}/logs/${date:format=yyyy-MM-dd}-webapi.log" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="logfile" />
  </rules>
</nlog>
```

### Log Csv
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <variable name="applicationName" value="MyApplication"/>
  <targets>
    <target name="asyncFile"
            xsi:type="AsyncWrapper"
            queueLimit="5000"
            overflowAction="Discard">
      <target name="file"
              xsi:type="File"
              fileName="${basedir}/logs/${applicationName}/log.csv"
              archiveFileName="${basedir}/logs/${applicationName}/log.{######}.csv"
              maxArchiveFiles="180"
              archiveEvery="Hour"
              archiveNumbering="Sequence"
              concurrentWrites="true"
              keepFileOpen="false"
              encoding="iso-8859-2">
        <layout xsi:type="CsvLayout">
          <column name="Type" layout="${level}"/>
          <column name="DateTime" layout="${date}" />
          <column name="Custom-Message" layout="${message}" />
          <column name="Error-Source" layout="${event-context:item=error-source}" />
          <column name="Error-Class" layout="${event-context:item=error-class}" />
          <column name="Error-Method" layout="${event-context:item=error-method}" />
          <column name="Error-Message" layout="${event-context:item=error-message}" />
          <column name="Inner-Error-Message" layout="${event-context:item=inner-error-message}" />
          <column name="Web-Variables" layout="${web_variables}" />
        </layout>
      </target>
    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Debug" writeTo="file" />
  </rules>
</nlog>
```

### Log Email
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	  autoReload="true">
  <targets>
    <target name="console"
            xsi:type="ColoredConsole"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}" />
    <target name="debug"
            xsi:type="Debugger"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}" />
    <target name="logfile"
            xsi:type="File"
            layout="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}"
            fileName="${basedir}/logs/${date:format=yyyy-MM-dd}-webapi.log" />
    <target name="Mail"
         xsi:type="Mail" html="true"
         subject="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}"
         body="Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}"
         to="recipient@sample.com"
         from="webmaster@sample.com"
         Encoding="UTF-8"
         smtpUsername="webmaster@sample.com"
         enableSsl="true"
         smtpPassword="123456"
         smtpAuthentication="Basic"
         smtpServer="smtp-relay.sample.com"
         smtpPort="587" />
  </targets>
  <rules>
    <logger name="*" level="Fatal" writeTo="Mail" />
    <logger name="*" level="Error" writeTo="Mail" />
    <logger name="*" minlevel="Trace" writeTo="console,debug" />
    <logger name="*" minlevel="Trace" writeTo="logfile" />
  </rules>
</nlog>
```

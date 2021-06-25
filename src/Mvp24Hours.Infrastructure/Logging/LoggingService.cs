//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Infrastructure.Logging.Renderer;
using NLog;
using NLog.Config;
using System;

namespace Mvp24Hours.Infrastructure.Logging
{
    /// <summary>
    /// Provides logging interface and utility functions.
    /// </summary>
    public class LoggingService : Logger, ILoggingService
    {
        private const string _loggerName = "NLogLogger";

        /// <summary>
        /// Get pre-configured instant log
        /// </summary>
        public static ILoggingService GetLoggingService()
        {
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("utc_date", typeof(UtcDateRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("web_variables", typeof(WebVariablesRenderer));
            return (ILoggingService)LogManager.GetLogger("NLogLogger", typeof(LoggingService));
        }

        public new void Debug(Exception exception, string format, params object[] args)
        {
            if (!base.IsDebugEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Debug, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public new void Error(Exception exception, string format, params object[] args)
        {
            if (!base.IsErrorEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Error, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public new void Fatal(Exception exception, string format, params object[] args)
        {
            if (!base.IsFatalEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Fatal, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public new void Info(Exception exception, string format, params object[] args)
        {
            if (!base.IsInfoEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Info, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public new void Trace(Exception exception, string format, params object[] args)
        {
            if (!base.IsTraceEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Trace, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public new void Warn(Exception exception, string format, params object[] args)
        {
            if (!base.IsWarnEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(_loggerName, LogLevel.Warn, exception, format, args);
            base.Log(typeof(LoggingService), logEvent);
        }

        public void Debug(Exception exception)
        {
            this.Debug(exception, string.Empty);
        }

        public void Error(Exception exception)
        {
            this.Error(exception, string.Empty);
        }

        public void Fatal(Exception exception)
        {
            this.Fatal(exception, string.Empty);
        }

        public void Info(Exception exception)
        {
            this.Info(exception, string.Empty);
        }

        public void Trace(Exception exception)
        {
            this.Trace(exception, string.Empty);
        }

        public void Warn(Exception exception)
        {
            this.Warn(exception, string.Empty);
        }

        private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
        {
            string assemblyProp = string.Empty;
            string classProp = string.Empty;
            string methodProp = string.Empty;
            string messageProp = string.Empty;
            string innerMessageProp = string.Empty;

            var logEvent = new LogEventInfo
                (level, loggerName, string.Format(format, args));

            if (exception != null)
            {
                assemblyProp = exception.Source;
                classProp = exception.TargetSite.DeclaringType?.FullName ?? exception.TargetSite.ReflectedType?.FullName;
                methodProp = exception.TargetSite.Name;
                messageProp = exception.Message;

                if (exception.InnerException != null)
                {
                    innerMessageProp = exception.InnerException.Message;
                }
            }

            logEvent.Properties["error-source"] = assemblyProp;
            logEvent.Properties["error-class"] = classProp;
            logEvent.Properties["error-method"] = methodProp;
            logEvent.Properties["error-message"] = messageProp;
            logEvent.Properties["inner-error-message"] = innerMessageProp;

            return logEvent;
        }
    }
}

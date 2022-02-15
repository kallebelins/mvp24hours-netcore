//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Logging.Renderer;
using NLog;
using NLog.Config;
using System;
using System.Diagnostics;

namespace Mvp24Hours.Infrastructure.Logging
{
    /// <summary>
    /// Provides logging interface and utility functions.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private static readonly ILoggingService _loggingService;
        private static readonly Logger _logger;
        internal const string LOGGER_NAME = "Mvp24HoursLogger";

        #region [ Ctor ]

        static LoggingService()
        {
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("utc_date", typeof(UtcDateRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("web_variables", typeof(WebVariablesRenderer));

            _logger = LogManager.GetLogger(LOGGER_NAME);
            _logger.Info("Starting the log engine.");
            _loggingService = new LoggingService();
        }

        ~LoggingService()
        {
            _logger.Info("Shutting down log engine.");
            LogManager.Shutdown();
        }

        #endregion

        public static ILoggingService GetLoggingService()
        {
            return _loggingService;
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }
        public void Debug(Exception exception)
        {
            Debug(exception, string.Empty);
        }
        public void Debug(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsDebugEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Debug, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }
        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }
        public void Error(Exception exception)
        {
            Error(exception, string.Empty);
        }
        public void Error(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsErrorEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Error, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }
        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }
        public void Fatal(Exception exception)
        {
            Fatal(exception, string.Empty);
        }
        public void Fatal(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsFatalEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Fatal, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }
        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }
        public void Info(Exception exception)
        {
            Info(exception, string.Empty);
        }
        public void Info(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsInfoEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Info, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }
        public void Trace(string message, params object[] args)
        {
            _logger.Trace(message, args);
        }
        public void Trace(Exception exception)
        {
            Trace(exception, string.Empty);
        }
        public void Trace(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsTraceEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Trace, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }
        public void Warn(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }
        public void Warn(Exception exception)
        {
            Warn(exception, string.Empty);
        }
        public void Warn(Exception exception, string format, params object[] args)
        {
            if (!_logger.IsWarnEnabled)
            {
                return;
            }

            var logEvent = GetLogEvent(LOGGER_NAME, LogLevel.Warn, exception, format, args);
            _logger.Log(typeof(LoggingService), logEvent);
        }

        private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
        {
            string assemblyProp = string.Empty;
            string classProp = string.Empty;
            string methodProp = string.Empty;
            string messageProp = string.Empty;
            string innerMessageProp = string.Empty;
            string stackTraceProp = string.Empty;

            var logEvent = new LogEventInfo
                (level, loggerName, string.Format(format, args));

            if (exception != null)
            {
                assemblyProp = exception.Source;
                classProp = exception.TargetSite.DeclaringType?.FullName ?? exception.TargetSite.ReflectedType?.FullName;
                methodProp = exception.TargetSite.Name;
                messageProp = exception.Message;
                stackTraceProp = exception.StackTrace;

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
            logEvent.Properties["error-stack-trace"] = stackTraceProp;

            // activity
            var activity = Activity.Current;
            if (activity != null)
            {
                logEvent.Properties["ActivityId"] = activity.Id;
                logEvent.Properties["TraceId"] = activity.GetTraceId();
                logEvent.Properties["SpanId"] = activity.GetSpanId();
                logEvent.Properties["ParentId"] = activity.GetParentId();
            }

            return logEvent;
        }
    }
}

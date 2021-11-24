//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class ServiceLoggingBuilderExtensions
    {
        /// <summary>
        /// Adds ConsoleTraceListener logging to the logging listeners.
        /// </summary>
        public static IServiceCollection UseMvp24HoursConsoleLogging(this IServiceCollection services)
        {
            ConsoleTarget target = new();
            AddNLogTarget(target, "Mvp24HoursConsole");
            return services;
        }

        /// <summary>
        /// Adds network logging.
        /// </summary>
        public static IServiceCollection UseMvp24HoursLog2ConsoleLogging(this IServiceCollection services)
        {
            ChainsawTarget target = new()
            {
                AppInfo = "Mvp24Hours",
                Address = "udp://127.0.0.1:7071"
            };

            AddNLogTarget(target, "Mvp24HoursLog2Console");
            return services;
        }

        /// <summary>
        /// Adds rolling file logging.
        /// </summary>
        public static IServiceCollection UseMvp24HoursFileLogging(this IServiceCollection services)
        {
            FileTarget target = new()
            {
                FileName = @"${basedir}/logs/${shortdate}.log",
                KeepFileOpen = false,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
            AddNLogTarget(target, "Mvp24HoursFile");
            return services;
        }

        /// <summary>
        /// Assigns the target to the Mvp24Hours rule and sets its layout to [longdate] [utc_date] [level] [message] [error-source] [error-class] [error-method] [error-message] [inner-error-message]
        /// </summary>
        private static void AddNLogTarget(TargetWithLayout target, string name, string defaultLayout = "Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}")
        {
            target.Layout = defaultLayout;
            target.Name = name;

            if (!LogManager.Configuration.AllTargets.Contains(target))
            {
                LogManager.Configuration.AddTarget(name, target);

                LoggingRule rule = new(LoggingService.LOGGER_NAME, target);

                if (!LogManager.Configuration.LoggingRules.Contains(rule))
                {
                    LogManager.Configuration.LoggingRules.Add(rule);
                }
            }
        }
    }
}

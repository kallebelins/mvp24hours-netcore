//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Hosting;
using Mvp24Hours.Infrastructure.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Mvp24Hours.Extensions
{
    public static class ServiceLoggingBuilderExtensions
    {
        /// <summary>
        /// Adds ConsoleTraceListener logging to the logging listeners.
        /// </summary>
        public static IHostBuilder UseMvp24HoursLoggingConsole(this IHostBuilder builder, string layout = null)
        {
            ConsoleTarget target = new("Mvp24HoursConsole");
            AddNLogTarget(target, layout);
            return builder;
        }

        /// <summary>
        /// Adds rolling file logging.
        /// </summary>
        public static IHostBuilder UseMvp24HoursFileLogging(this IHostBuilder builder, string fileName = null, string layout = null)
        {
            FileTarget target = new("Mvp24HoursFile")
            {
                FileName = fileName ?? @"${basedir}/logs/${shortdate}.log",
                KeepFileOpen = false,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
            AddNLogTarget(target, layout);
            return builder;
        }

        /// <summary>
        /// Assigns the target to the Mvp24Hours rule and sets its layout to [longdate] [utc_date] [level] [message] [error-source] [error-class] [error-method] [error-message] [inner-error-message]
        /// </summary>
        private static void AddNLogTarget(TargetWithLayout target, string defaultLayout = null)
        {
            target.Layout = defaultLayout ?? "Server-Date: ${longdate}; UTC-Date: ${utc_date}; Level: ${level}; Log-Message: ${message}; Error-Source: ${event-context:item=error-source}; Error-Class: ${event-context:item=error-class}; Error-Method: ${event-context:item=error-method}; Error-Message: ${event-context:item=error-message}; Inner-Error-Message: ${event-context:item=inner-error-message}";

            if (!LogManager.Configuration.AllTargets.Contains(target))
            {
                LogManager.Configuration.AddTarget(target);

                LoggingRule rule = new(LoggingService.LOGGER_NAME, target);

                if (!LogManager.Configuration.LoggingRules.Contains(rule))
                {
                    LogManager.Configuration.LoggingRules.Add(rule);
                }
            }
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// Class that helps reading API settings
    /// </summary>
    public class ConfigurationHelper
    {
        #region [ Envionment ]

        private static IHostEnvironment _environment;

        /// <summary>
        /// Defines the host environment of the application that is running
        /// </summary>
        public static void SetEnvironment(IHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the host environment of the application that is running
        /// </summary>
        public static IHostEnvironment GetEnvironment()
        {
            return _environment;
        }

        #endregion

        #region [ Settings ]

        private static IConfigurationRoot _appSettings;

        internal static IConfigurationRoot AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    var env = GetEnvironment();
                    if (env == null) return null;

                    IConfigurationBuilder builder = new ConfigurationBuilder();
                    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true, reloadOnChange: true);
                    builder.AddJsonFile($"appsettings.{GetEnvironment().EnvironmentName}.json", optional: true, reloadOnChange: true);
                    _appSettings = builder.Build();
                }
                return _appSettings;
            }
        }

        /// <summary>
        /// Get the settings of the application that is running
        /// </summary>
        public static string GetSettings(string key)
        {
            if (AppSettings != null)
            {
                return AppSettings?.GetSection(key)?.Value;
            }
            else if (_configuration != null)
            {
                return _configuration.GetSection(key)?.Value;
            }
            return string.Empty;
        }

        #region [ Configuration Settings ]

        private static IConfiguration _configuration;

        /// <summary>
        /// Records native .NET core configuration
        /// </summary>
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #endregion
    }
}

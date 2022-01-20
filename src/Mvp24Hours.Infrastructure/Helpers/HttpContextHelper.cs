//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// Contains functions to register and obtain context of the application that is running
    /// </summary>
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Defines individual HTTP request context
        /// </summary>
        public static void SetContext(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        /// <summary>
        /// Get individual HTTP request context
        /// </summary>
        public static HttpContext GetContext()
        {
            return httpContextAccessor?.HttpContext;
        }
        /// <summary>
        /// Gets IP of the user who originated the current request
        /// </summary>
        public static string GetUserIP()
        {
            var context = GetContext();

            if (context != null)
            {
                string ip = context.Connection?.RemoteIpAddress?.ToString() ?? context.Connection?.LocalIpAddress?.ToString();
                if (ip.Contains(":"))
                {
                    ip = ip.Split(':').First().Trim();
                }

                if (ip.Contains(","))
                {
                    ip = ip.Split(',').First().Trim();
                }

                if (!string.IsNullOrEmpty(ip))
                {
                    return ip;
                }
                else
                {
                    return "127.0.0.1";
                }
            }
            return "0.0.0.0";
        }
        /// <summary>
        /// Get web address dynamically from current service
        /// </summary>
        public static string GetBaseUrl()
        {
            return $"{GetContext().Request.Scheme}://{GetContext().Request.Host.Value}{GetContext().Request.PathBase.Value}";
        }
    }
}

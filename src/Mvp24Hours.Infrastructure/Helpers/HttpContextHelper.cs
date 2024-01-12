//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;

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
            ServiceProviderHelper.SetProvider((state) =>
            {
                if (state is IHttpContextAccessor http)
                    return http.HttpContext?.RequestServices;
                return null;
            }, accessor);
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
                string ip = context.Connection?.RemoteIpAddress?.ToString() ?? context.Connection?.LocalIpAddress?.ToString() ?? "127.0.0.1";

                if (ip.Contains(":"))
                {
                    ip = ip.Split(':')[0].Trim();
                }

                if (ip.Contains(","))
                {
                    ip = ip.Split(',')[0].Trim();
                }

                return ip;
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

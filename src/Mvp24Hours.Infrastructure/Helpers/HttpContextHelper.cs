//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class HttpContextHelper
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static void SetContext(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static HttpContext GetContext()
        {
            return httpContextAccessor?.HttpContext;
        }

        public static T GetService<T>()
        {
            return (T)GetContext().RequestServices?.GetService(typeof(T));
        }

        public static string GetUserIP()
        {
            var context = HttpContextHelper.GetContext();

            if (context != null)
            {
                string ip = context.Connection?.RemoteIpAddress?.ToString() ?? context.Connection?.LocalIpAddress?.ToString();
                if (ip.Contains(":"))
                    ip = ip.Split(':').First().Trim();
                if (ip.Contains(","))
                    ip = ip.Split(',').First().Trim();
                if (!string.IsNullOrEmpty(ip))
                    return ip;
                else
                    return "127.0.0.1";
            }
            return "0.0.0.0";
        }

        public static string GetBaseUrl()
        {
            return $"{GetContext().Request.Scheme}://{GetContext().Request.Host.Value.ToString()}{GetContext().Request.PathBase.Value.ToString()}";
        }
    }
}

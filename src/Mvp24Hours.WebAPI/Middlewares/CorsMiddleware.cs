//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Middlewares
{
    /// <summary>
    /// See: https://stackoverflow.com/a/45844400
    /// </summary>
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string credentialsCors = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:Cors:Credentials");

            if (credentialsCors.HasValue())
            {
                context.Response.Headers.Add("Access-Control-Allow-Credentials", credentialsCors);
            }

            string allCors = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:Cors:All");
            string originCors, headersCors, methodsCors;

            if (allCors.HasValue())
            {
                originCors = allCors;
                headersCors = allCors;
                methodsCors = allCors;
            }
            else
            {
                originCors = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:Cors:Origin");
                headersCors = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:Cors:Headers");
                methodsCors = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:Cors:Methods");
            }

            if (originCors.HasValue())
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", originCors);
            }
            // Added "Accept-Encoding" to this list
            if (headersCors.HasValue())
            {
                context.Response.Headers.Add("Access-Control-Allow-Headers", headersCors);
            }

            if (methodsCors.HasValue())
            {
                context.Response.Headers.Add("Access-Control-Allow-Methods", methodsCors);
            }

            // New Code Starts here
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(string.Empty);
            }
            // New Code Ends here

            await _next(context);
        }
    }
}

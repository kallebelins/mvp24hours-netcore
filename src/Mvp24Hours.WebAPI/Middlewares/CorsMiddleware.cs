//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;
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
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            // Added "Accept-Encoding" to this list
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "*");

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

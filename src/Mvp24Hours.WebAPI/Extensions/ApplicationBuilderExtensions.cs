//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Middlewares;
using Mvp24Hours.WebAPI.Middlewares;
using System;

namespace Mvp24Hours.WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCaching(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CachingMiddleware>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDocumentation(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Integração Meios de Hospedagem - v1");
            });
            return builder;
        }

        /// <summary>
        /// Configure application context
        /// </summary>
        public static IApplicationBuilder UseMvp24Hours(this IApplicationBuilder app)
        {
            IServiceProvider service = app.ApplicationServices;

            service.GetService<IHostEnvironment>()
                .AddEnvironment();

            service.GetService<IHttpContextAccessor>()
                .AddContext();

            return app;
        }

        private static IHttpContextAccessor AddContext(this IHttpContextAccessor accessor)
        {
            HttpContextHelper.SetContext(accessor);
            return accessor;
        }

        private static IHostEnvironment AddEnvironment(this IHostEnvironment env)
        {
            ConfigurationHelper.SetEnvironment(env);
            return env;
        }
    }
}

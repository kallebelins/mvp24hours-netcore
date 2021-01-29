//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Data;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.WebAPI.Filters;
using System;

namespace Mvp24Hours.WebAPI.Extensions
{
    public static class StartupExtensions
    {
        #region [ Configure]

        /// <summary>
        /// Use in Configure(IApplicationBuilder app, ...)
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

        #endregion

        #region [ ConfigureServices ]

        /// <summary>
        /// Use in ConfigureServices(IServiceCollection services)
        /// </summary>
        public static IServiceCollection AddMvp24HoursService(this IServiceCollection services, Type TDbContext = null)
        {
            #region [ HttpContext ]
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region [ Notification ]
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddMvc(options => options.Filters.Add<NotificationFilter>());
            #endregion

            #region [ Persistence ]
            if (TDbContext != null)
            {
                services.AddScoped(typeof(DbContext), TDbContext);
                services.AddScoped<IUnitOfWork>(x => new UnitOfWork());
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            }
            #endregion

            #region [ Hateaos ]
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            #endregion

            return services;
        }

        /// <summary>
        /// Use in ConfigureServices(IServiceCollection services)
        /// </summary>
        public static IServiceCollection AddMvp24HoursService<TDbContext>(this IServiceCollection services)
        {
            return AddMvp24HoursService(services, typeof(TDbContext));
        }

        #endregion
    }
}

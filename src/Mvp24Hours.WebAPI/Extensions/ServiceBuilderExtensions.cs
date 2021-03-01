using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Data;
using Mvp24Hours.WebAPI.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO.Compression;
using System.Reflection;

namespace Mvp24Hours.WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceBuilderExtensions
    {
        /// <summary>
        /// Add all services
        /// </summary>
        public static IServiceCollection AddMvp24HoursAll<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory, Assembly assemblyMap = null)
            where TDbContext : DbContext
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursDbService(dbFactory);
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
            services.AddMvp24HoursZipService();
            return services;
        }

        /// <summary>
        /// Add all services
        /// </summary>
        public static IServiceCollection AddMvp24HoursAll(this IServiceCollection services, Assembly assemblyMap = null)
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
            services.AddMvp24HoursZipService();
            return services;
        }

        /// <summary>
        /// Add all services async
        /// </summary>
        public static IServiceCollection AddMvp24HoursAllAsync<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory, Assembly assemblyMap = null)
            where TDbContext : DbContext
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursDbAsyncService(dbFactory);
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
            services.AddMvp24HoursZipService();
            return services;
        }

        /// <summary>
        /// Add all services async
        /// </summary>
        public static IServiceCollection AddMvp24HoursAllAsync(this IServiceCollection services, Assembly assemblyMap = null)
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
            services.AddMvp24HoursZipService();
            return services;
        }

        /// <summary>
        /// Adds essential services
        /// </summary>
        public static IServiceCollection AddMvp24HoursService(this IServiceCollection services)
        {
            #region [ HttpContext ]
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region [ Notification ]
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddMvc(options => options.Filters.Add<NotificationFilter>());
            #endregion

            #region [ Hateaos ]
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            #endregion

            return services;
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursDbAsyncService<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null)
            where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWorkAsync>(x => new UnitOfWorkAsync());
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            if (dbFactory != null)
                services.AddScoped<DbContext>(dbFactory);
            else
                services.AddScoped<DbContext, TDbContext>();

            return services;
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursDbService<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null)
               where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork>(x => new UnitOfWork());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            if (dbFactory != null)
                services.AddScoped<DbContext>(dbFactory);
            else
                services.AddScoped<DbContext, TDbContext>();


            return services;
        }

        /// <summary>
        /// Add mapping services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMapService(this IServiceCollection services, Assembly assemblyMap)
        {
            Assembly local = assemblyMap ?? Assembly.GetExecutingAssembly();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile(local));
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursJsonService(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyy-MM-dd"
            };
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursZipService(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            return services;
        }
    }
}

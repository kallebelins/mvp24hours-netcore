//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Data;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.WebAPI.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
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
        public static IServiceCollection AddMvp24HoursAll<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null, Assembly assemblyMap = null)
            where TDbContext : DbContext
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursDbService(dbFactory);
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
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
            return services;
        }

        /// <summary>
        /// Add all services async
        /// </summary>
        public static IServiceCollection AddMvp24HoursAllAsync<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null, Assembly assemblyMap = null)
            where TDbContext : DbContext
        {
            services.AddMvp24HoursService();
            services.AddMvp24HoursDbAsyncService(dbFactory);
            services.AddMvp24HoursMapService(assemblyMap);
            services.AddMvp24HoursJsonService();
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

            #region [ Filters ]
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IHATEOASContext, HATEOASContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add<NotificationFilter>();
                options.Filters.Add<HATEOASFilter>();
            });

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
        public static IServiceCollection AddMvp24HoursZipService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = (bool)configuration.GetSection("Mvp24Hours:Web:ResponseCompressionForHttps")?.ToString()?.ToBoolean(false);
                options.Providers.Add<GzipCompressionProvider>();
            });

            return services;
        }

        /// <summary>
        /// Veja as configurações em: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
        /// </summary>
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetSection("Mvp24Hours:Persistence:Redis")?.Get<ConfigurationOptions>();

            if (redisConfiguration == null)
            {
                throw new ArgumentNullException("Redis configuration not defined.");
            }

            services.AddDistributedRedisCache(options =>
            {
                options.ConfigurationOptions = redisConfiguration;
            });

            return services;
        }

        /// <summary>
        /// Veja as configurações em: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
        /// </summary>
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string instanceName = null)
        {
            var redisConfiguration = configuration.GetSection("Mvp24Hours:Persistence:Redis")?.Get<ConfigurationOptions>();

            if (redisConfiguration == null)
            {
                redisConfiguration = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    AllowAdmin = true,
                    ConnectRetry = 2,
                    ConnectTimeout = 6000,
                    ResponseTimeout = 6000,
                    Ssl = false                    
                };
            }

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString(connectionStringName);
                options.InstanceName = instanceName;
                options.ConfigurationOptions = redisConfiguration;
            });

            return services;
        }

    }
}

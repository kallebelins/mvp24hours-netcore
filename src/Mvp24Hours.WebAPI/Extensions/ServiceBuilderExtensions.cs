using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Data;
using Mvp24Hours.WebAPI.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO.Compression;

namespace Mvp24Hours.WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceBuilderExtensions
    {
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
        public static IServiceCollection AddMvp24HoursDbAsyncService(this IServiceCollection services, params Type[] contextTypes)
        {
            foreach (var type in contextTypes)
            {
                services.AddScoped(type, type);
            }

            services.AddScoped<IUnitOfWorkAsync>(x => new UnitOfWorkAsync());
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            return services;
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursDbService(this IServiceCollection services, params Type[] contextTypes)
        {
            foreach (var type in contextTypes)
            {
                services.AddScoped(type, type);
            }

            services.AddScoped<IUnitOfWork>(x => new UnitOfWork());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        /// <summary>
        /// Add business services (result / criteria / paging)
        /// </summary>
        public static IServiceCollection AddMvp24HoursBsService(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IBusinessResult<>), typeof(BusinessResult<>));
            services.AddSingleton(typeof(ILinkResult), typeof(LinkResult));
            services.AddSingleton(typeof(IMessageResult), typeof(MessageResult));
            services.AddSingleton(typeof(IPageResult), typeof(PageResult));
            services.AddSingleton(typeof(IPagingCriteria), typeof(PagingCriteria));
            services.AddSingleton(typeof(IPagingCriteriaExpression<>), typeof(PagingCriteriaExpression<>));
            services.AddSingleton(typeof(IPagingResult<>), typeof(PagingResult<>));
            services.AddSingleton(typeof(ISummaryResult), typeof(SummaryResult));

            return services;
        }

        /// <summary>
        /// Add mapping services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMapService(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
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

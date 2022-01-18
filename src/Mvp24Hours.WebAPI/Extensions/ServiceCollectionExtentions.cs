//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.WebAPI.Filters;
using Mvp24Hours.WebAPI.Filters.Swagger;
using Mvp24Hours.WebAPI.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace Mvp24Hours.WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtentions
    {
        /// <summary>
        /// Adds IHttpContextAccessor and IActionContextAccessor
        /// </summary>
        public static IServiceCollection AddMvp24HoursWebEssential(this IServiceCollection services)
        {
            services.AddMvp24HoursEssential();

            if (!services.Exists<IHttpContextAccessor>())
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
            if (!services.Exists<IActionContextAccessor>())
            {
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            }
            return services;
        }

        /// <summary>
        /// Adds filters HateoasFilter and NotificationFilter
        /// </summary>
        public static IServiceCollection AddMvp24HoursWebFilters(this IServiceCollection services, bool enableHateoas = false)
        {
            if (!services.Exists<IHttpContextAccessor>())
            {
                throw new ArgumentNullException("IHttpContextAccessor context not found.");
            }

            if (enableHateoas && !services.Exists<IHateoasContext>())
            {
                services.AddScoped<IHateoasContext, HateoasContext>();
                services.AddMvc(options =>
                {
                    options.Filters.Add<HateoasFilter>();
                });
                if (!services.Exists<IActionContextAccessor>())
                {
                    throw new ArgumentNullException("IActionContextAccessor context not found.");
                }
            }

            return services;
        }

        /// <summary>
        /// Add json serialization
        /// </summary>
        public static IServiceCollection AddMvp24HoursWebJson(this IServiceCollection services, JsonSerializerSettings jsonSerializerSettings = null)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).ContractResolver;
                    options.SerializerSettings.Converters = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).Converters;
                    options.SerializerSettings.DateFormatHandling = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).DateFormatHandling;
                    options.SerializerSettings.DateFormatString = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).DateFormatString;
                    options.SerializerSettings.NullValueHandling = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).NullValueHandling;
                    options.SerializerSettings.ReferenceLoopHandling = (jsonSerializerSettings ?? JsonHelper.JsonDefaultSettings).ReferenceLoopHandling;
                });
            return services;
        }

        /// <summary>
        /// Add configuration for GzipCompressionProvider
        /// </summary>
        public static IServiceCollection AddMvp24HoursWebGzip(this IServiceCollection services, bool enableForHttps = false)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = enableForHttps;
                options.Providers.Add<GzipCompressionProvider>();
            });

            return services;
        }

        /// <summary>
        /// Add configuration for Swagger
        /// </summary>
        public static IServiceCollection AddMvp24HoursWebSwagger(this IServiceCollection services,
            string title, string version = "v1", string xmlCommentsFileName = null,
            bool enableExample = false, SwaggerAuthorizationScheme oAuthScheme = SwaggerAuthorizationScheme.None,
            IEnumerable<Type> authTypes = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

                if (enableExample)
                {
                    c.ExampleFilters();
                    c.OperationFilter<AddResponseHeadersFilter>();
                }

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                if (oAuthScheme == SwaggerAuthorizationScheme.Bearer)
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    if (authTypes == null)
                    {
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                            {
                                new OpenApiSecurityScheme {
                                    Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new List<string>()
                            }
                        });
                    }
                }
                else if (oAuthScheme == SwaggerAuthorizationScheme.Basic)
                {
                    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                    {
                        Description = @"Authorization header using the Basic scheme. \r\n\r\n 
                          Enter 'Basic' [space] and then your token in the text input below.
                          \r\n\r\nExample: 'Basic 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Basic"
                    });

                    if (authTypes == null)
                    {
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                            {
                                new OpenApiSecurityScheme {
                                    Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Basic"
                                    },
                                },
                                new List<string>()
                            }
                        });
                    }
                }

                if (oAuthScheme != SwaggerAuthorizationScheme.None && authTypes != null)
                {
                    c.OperationFilter<AuthResponsesOperationFilter>(authTypes);
                }

                // Set the comments path for the Swagger JSON and UI.
                if (!string.IsNullOrEmpty(xmlCommentsFileName))
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });

            if (enableExample)
            {
                services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            }

            return services;
        }
    }
}

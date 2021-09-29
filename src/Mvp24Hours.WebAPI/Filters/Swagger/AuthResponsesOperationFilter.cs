//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Mvp24Hours.Infrastructure.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.WebAPI.Filters.Swagger
{
    /// <summary>
    /// Remove lock icon from service
    /// </summary>
    /// <remarks>
    /// Add to swagger service registry => c.OperationFilter&lt;AuthResponsesOperationFilter&lt;AuthorizeAttribute&gt;&gt;();
    /// </remarks>
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public IEnumerable<Type> AuthTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthResponsesOperationFilter(IEnumerable<Type> authTypes)
        {
            AuthTypes = authTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!AuthTypes.AnyOrNotNull())
            {
                return;
            }

            var hasAuthAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .Where(x => AuthTypes.Contains(x.GetType()) && !x.GetType().Equals(typeof(AllowAnonymousAttribute)))
                .AnyOrNotNull();

            var hasAllowAnonymousAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .Where(x => x.GetType().Equals(typeof(AllowAnonymousAttribute)))
                .AnyOrNotNull();

            if (hasAuthAttributes && !hasAllowAnonymousAttributes)
            {
                var securityRequirement = new OpenApiSecurityRequirement()
                {
                    {
                        // Put here you own security scheme, this one is an example
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                };
                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }
        }
    }
}

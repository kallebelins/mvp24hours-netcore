//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Helpers;
using System;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceBuilderExtensions
    {
        /// <summary>
        /// Create a ServiceProvider
        /// </summary>
        public static IServiceCollection UseMvp24Hours(this IServiceCollection services, ServiceProviderOptions options = null, bool scoped = false)
        {
            IServiceProvider provider =
                (options == null)
                    ? services.BuildServiceProvider()
                        : services.BuildServiceProvider(options);

            ServiceProviderHelper.SetProvider(provider);

            if (scoped)
            {
                ServiceProviderHelper.SetProvider((scope) =>
                {
                    if (scope is IServiceScope service)
                        return service.ServiceProvider;
                    return null;
                }, provider.CreateScope());
            }

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.Caching.Helpers;
using System;

namespace Mvp24Hours.Extensions
{
    public static class CachingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursCaching(this IServiceCollection services,
            DateTimeOffset? AbsoluteExpiration = null,
            TimeSpan? AbsoluteExpirationRelativeToNow = null,
            TimeSpan? SlidingExpiration = null)
        {
            CacheConfigHelper.AbsoluteExpiration = AbsoluteExpiration;
            CacheConfigHelper.AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow;
            CacheConfigHelper.SlidingExpiration = SlidingExpiration;
            return services;
        }
    }
}

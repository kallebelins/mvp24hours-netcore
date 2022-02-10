//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Infrastructure.Caching.Helpers;
using System;

namespace Mvp24Hours.Extensions
{
    public static class CacheExtensions
    {
        public static void SetString(this IDistributedCache cache, string key, string value, int minutes)
        {
            if (cache == null || !key.HasValue() || !value.HasValue())
            {
                return;
            }
            cache.SetString(key, value, DateTimeOffset.Now.AddMinutes(minutes));
        }

        public static void SetString(this IDistributedCache cache, string key, string value, DateTimeOffset time)
        {
            if (cache == null || !key.HasValue() || !value.HasValue())
            {
                return;
            }
            cache.SetString(key, value, CacheConfigHelper.GetCacheOptions(time));
        }
    }
}

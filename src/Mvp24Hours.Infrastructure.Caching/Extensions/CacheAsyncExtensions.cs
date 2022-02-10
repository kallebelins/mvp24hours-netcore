//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Infrastructure.Caching.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    public static class CacheAsyncExtensions
    {
        public static async Task SetStringAsync(this IDistributedCache cache, string key, string value, int minutes, CancellationToken token = default)
        {
            if (cache == null || !key.HasValue() || !value.HasValue())
            {
                return;
            }
            await cache.SetStringAsync(key, value, DateTimeOffset.Now.AddMinutes(minutes), token);
        }

        public static async Task SetStringAsync(this IDistributedCache cache, string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            if (cache == null || !key.HasValue() || !value.HasValue())
            {
                return;
            }
            await cache.SetStringAsync(key, value, CacheConfigHelper.GetCacheOptions(time), token);
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class RedisCacheExtensions
    {
        public static DistributedCacheEntryOptions GetRedisCacheOptions(DateTimeOffset? time = default)
        {
            return new DistributedCacheEntryOptions { AbsoluteExpiration = time ?? DateTimeOffset.Now.AddMinutes(5) };
        }

        public static async Task<string> GetRedisStringAsync(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            return await cache.GetStringAsync(key, token);
        }

        public static async Task SetRedisStringAsync(this IDistributedCache cache, string key, string value, CancellationToken token = default)
        {
            await cache.SetRedisStringAsync(key, value, DateTimeOffset.Now.AddMinutes(5), token);
        }

        public static async Task SetRedisStringAsync(this IDistributedCache cache, string key, string value, int minutes, CancellationToken token = default)
        {
            await cache.SetRedisStringAsync(key, value, DateTimeOffset.Now.AddMinutes(minutes), token);
        }

        public static async Task SetRedisStringAsync(this IDistributedCache cache, string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            await cache.SetStringAsync(key, value, GetRedisCacheOptions(time), token);
        }

        public static async Task RemoveRedisStringAsync(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            await cache.RemoveAsync(key, token);
        }
    }
}

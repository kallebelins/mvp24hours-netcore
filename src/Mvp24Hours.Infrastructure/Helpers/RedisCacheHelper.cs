using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Infrastructure.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class RedisCacheHelper
    {
        private static IDistributedCache _redisCache;
        public static IDistributedCache RedisCache
        {
            get
            {
                return _redisCache ??= ServiceProviderHelper.GetService<IDistributedCache>();
            }
        }

        public static async Task<string> GetStringAsync(string key, CancellationToken token = default)
        {
            return await RedisCache.GetRedisStringAsync(key, token);
        }

        public static async Task SetStringAsync(string key, string value, CancellationToken token = default)
        {
            await RedisCache.SetRedisStringAsync(key, value, token);
        }

        public static async Task SetStringAsync(string key, string value, int minutes, CancellationToken token = default)
        {
            await RedisCache.SetRedisStringAsync(key, value, minutes, token);
        }

        public static async Task SetStringAsync(string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            await RedisCache.SetRedisStringAsync(key, value, time, token);
        }

        public static async Task RemoveStringAsync(string key, CancellationToken token = default)
        {
            await RedisCache.RemoveRedisStringAsync(key, token);
        }
    }
}

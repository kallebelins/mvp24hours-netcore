//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    public static class ObjectCacheAsyncExtensions
    {
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
            where T : class
        {
            if (cache == null || key.HasValue())
            {
                return default;
            }
            string value = await cache.GetStringAsync(key, token);
            if (!value.HasValue())
            {
                return default;
            }
            return value.ToDeserialize<T>(jsonSerializerSettings);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
            where T : class
        {
            if (cache == null || key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            await cache.SetStringAsync(key, result, token);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value, int minutes, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
            where T : class
        {
            if (cache == null || key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            await cache.SetStringAsync(key, result, minutes, token);
        }

        public static async Task SetObjectAsync(this IDistributedCache cache, string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (cache == null || key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            await cache.SetStringAsync(key, result, time, token);
        }
    }
}

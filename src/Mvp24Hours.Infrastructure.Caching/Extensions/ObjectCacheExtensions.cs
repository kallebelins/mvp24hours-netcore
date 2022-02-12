//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Mvp24Hours.Extensions
{
    public static class ObjectCacheExtensions
    {
        public static T GetObject<T>(this IDistributedCache cache, string key, JsonSerializerSettings jsonSerializerSettings = null)
            where T : class
        {
            if (cache == null || !key.HasValue())
            {
                return default;
            }
            string value = cache.GetString(key);
            if (!value.HasValue())
            {
                return default;
            }
            return value.ToDeserialize<T>(jsonSerializerSettings);
        }

        public static void SetObject<T>(this IDistributedCache cache, string key, T value, JsonSerializerSettings jsonSerializerSettings = null)
            where T : class
        {
            if (cache == null || !key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            cache.SetString(key, result);
        }

        public static void SetObject<T>(this IDistributedCache cache, string key, T value, int minutes, JsonSerializerSettings jsonSerializerSettings = null)
            where T : class
        {
            if (cache == null || !key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            cache.SetString(key, result, minutes);
        }

        public static void SetObject(this IDistributedCache cache, string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (cache == null || !key.HasValue() || value == null)
            {
                return;
            }
            string result = value.ToSerialize(jsonSerializerSettings);
            cache.SetString(key, result, time);
        }
    }
}

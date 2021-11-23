//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class RedisObjectCacheHelper
    {
        private static readonly ILoggingService _logger;

        private static IDistributedCache _redisCache;
        public static IDistributedCache RedisCache
        {
            get
            {
                return _redisCache ??= ServiceProviderHelper.GetService<IDistributedCache>();
            }
        }

#pragma warning disable S3963 // "static" fields should be initialized inline
        static RedisObjectCacheHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<T> GetObjectAsync<T>(string key, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (RedisCache == null)
            {
                return default;
            }

            try
            {
                return await RedisCache.GetRedisObjectAsync<T>(key, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default;
        }

        public static async Task SetObjectAsync<T>(string key, T value, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (RedisCache == null)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisObjectAsync<T>(key, value, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetObjectAsync(string key, string value, int minutes, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (RedisCache == null)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisObjectAsync(key, value, minutes, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetObjectAsync(string key, string value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (RedisCache == null)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisObjectAsync(key, value, time, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

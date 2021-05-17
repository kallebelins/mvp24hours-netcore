using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class RedisCacheHelper
    {
        private static readonly ILoggingService _logger;

        private static bool? _enableRedis;
        private static bool EnableRedis
        {
            get
            {
                if (_enableRedis == null)
                {
                    string value = ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:EnableRedis");
                    _enableRedis = value.ToBoolean(true);
                }
                return (bool)_enableRedis;
            }
        }

        private static IDistributedCache _redisCache;
        public static IDistributedCache RedisCache
        {
            get
            {
                return _redisCache ??= ServiceProviderHelper.GetService<IDistributedCache>();
            }
        }

        static RedisCacheHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }

        public static async Task<string> GetStringAsync(string key, CancellationToken token = default)
        {
            if (RedisCache == null || !EnableRedis)
            {
                return null;
            }

            try
            {
                return await RedisCache.GetRedisStringAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public static async Task SetStringAsync(string key, string value, CancellationToken token = default)
        {
            if (RedisCache == null || !EnableRedis)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisStringAsync(key, value, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetStringAsync(string key, string value, int minutes, CancellationToken token = default)
        {
            if (RedisCache == null || !EnableRedis)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisStringAsync(key, value, minutes, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetStringAsync(string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            if (RedisCache == null || !EnableRedis)
            {
                return;
            }

            try
            {
                await RedisCache.SetRedisStringAsync(key, value, time, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task RemoveStringAsync(string key, CancellationToken token = default)
        {
            if (RedisCache == null || !EnableRedis)
            {
                return;
            }

            try
            {
                await RedisCache.RemoveRedisStringAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

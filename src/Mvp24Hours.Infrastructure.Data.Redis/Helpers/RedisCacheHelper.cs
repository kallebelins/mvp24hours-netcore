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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class RedisCacheHelper
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
        static RedisCacheHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<string> GetStringAsync(string key, CancellationToken token = default)
        {
            if (RedisCache == null)
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
            if (RedisCache == null)
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
            if (RedisCache == null)
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
            if (RedisCache == null)
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
            if (RedisCache == null)
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

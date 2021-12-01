//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class CacheAsyncExtensions
    {
        private static bool? _enable;
        private static DateTimeOffset? _defaultExpiration;
        private static readonly ILoggingService _logger;

#pragma warning disable S3963 // "static" fields should be initialized inline
        static CacheAsyncExtensions()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        private static bool Enable
        {
            get
            {
                if (_enable == null)
                {
                    string value = ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:Cache:Enable");
                    _enable = value.ToBoolean(true);
                }
                return (bool)_enable;
            }
        }

        private static DateTimeOffset? DefaultExpiration
        {
            get
            {
                if (_defaultExpiration == null)
                {
                    string value = ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:Cache:DefaultExpiration");
                    _defaultExpiration = value.ToDateTime();
                }
                return _defaultExpiration;
            }
        }

        public static DistributedCacheEntryOptions GetCacheOptions(DateTimeOffset? time = default)
        {
            return new DistributedCacheEntryOptions { AbsoluteExpiration = time ?? DefaultExpiration };
        }

        public static async Task<string> GetCacheStringAsync(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            if (cache == null || !Enable)
            {
                return null;
            }

            try
            {
                return await cache.GetStringAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public static async Task SetCacheStringAsync(this IDistributedCache cache, string key, string value, CancellationToken token = default)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                await cache.SetStringAsync(key, value, GetCacheOptions(), token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetCacheStringAsync(this IDistributedCache cache, string key, string value, int minutes, CancellationToken token = default)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                await cache.SetCacheStringAsync(key, value, DateTimeOffset.Now.AddMinutes(minutes), token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetCacheStringAsync(this IDistributedCache cache, string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                await cache.SetStringAsync(key, value, GetCacheOptions(time), token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task RemoveCacheStringAsync(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                await cache.RemoveAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

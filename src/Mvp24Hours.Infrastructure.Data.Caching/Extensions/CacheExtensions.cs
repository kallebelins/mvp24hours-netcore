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

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class CacheExtensions
    {
        private static bool? _enable;
        private static DateTimeOffset? _defaultExpiration;
        private static readonly ILoggingService _logger;

#pragma warning disable S3963 // "static" fields should be initialized inline
        static CacheExtensions()
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

        public static string GetCacheString(this IDistributedCache cache, string key)
        {
            if (cache == null || !Enable)
            {
                return null;
            }

            try
            {
                return cache.GetString(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public static void SetCacheString(this IDistributedCache cache, string key, string value)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                cache.SetString(key, value, GetCacheOptions());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetCacheString(this IDistributedCache cache, string key, string value, int minutes)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                cache.SetCacheString(key, value, DateTimeOffset.Now.AddMinutes(minutes));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetCacheString(this IDistributedCache cache, string key, string value, DateTimeOffset time)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                cache.SetString(key, value, GetCacheOptions(time));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void RemoveCacheString(this IDistributedCache cache, string key)
        {
            if (cache == null || !Enable)
            {
                return;
            }

            try
            {
                cache.Remove(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

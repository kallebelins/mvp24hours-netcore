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
using System;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class CacheHelper
    {
        private static readonly ILoggingService _logger;

        private static IDistributedCache _Cache;
        public static IDistributedCache Cache
        {
            get
            {
                return _Cache ??= ServiceProviderHelper.GetService<IDistributedCache>();
            }
        }

#pragma warning disable S3963 // "static" fields should be initialized inline
        static CacheHelper()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static string GetString(string key)
        {
            if (Cache == null)
            {
                return null;
            }

            try
            {
                return Cache.GetCacheString(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public static void SetString(string key, string value)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheString(key, value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetString(string key, string value, int minutes)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheString(key, value, minutes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetString(string key, string value, DateTimeOffset time)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheString(key, value, time);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void RemoveString(string key)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.RemoveCacheString(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

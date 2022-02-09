//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Helpers
{
    public static class CacheAsyncHelper
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
        static CacheAsyncHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<string> GetStringAsync(string key, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return null;
            }

            try
            {
                return await Cache.GetCacheStringAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public static async Task SetStringAsync(string key, string value, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheStringAsync(key, value, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetStringAsync(string key, string value, int minutes, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheStringAsync(key, value, minutes, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetStringAsync(string key, string value, DateTimeOffset time, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheStringAsync(key, value, time, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task RemoveStringAsync(string key, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.RemoveCacheStringAsync(key, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

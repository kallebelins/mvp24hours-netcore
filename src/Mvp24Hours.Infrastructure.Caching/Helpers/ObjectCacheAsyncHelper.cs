//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Extensions;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Helpers
{
    public static class ObjectCacheAsyncHelper
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
        static ObjectCacheAsyncHelper()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<T> GetObjectAsync<T>(string key, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return default;
            }

            try
            {
                return await Cache.GetCacheObjectAsync<T>(key, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default;
        }

        public static async Task SetObjectAsync<T>(string key, T value, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheObjectAsync<T>(key, value, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetObjectAsync(string key, object value, int minutes, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheObjectAsync(key, value, minutes, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetObjectAsync(string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                await Cache.SetCacheObjectAsync(key, value, time, jsonSerializerSettings, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

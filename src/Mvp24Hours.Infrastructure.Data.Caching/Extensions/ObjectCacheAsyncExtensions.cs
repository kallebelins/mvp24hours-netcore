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
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class ObjectCacheAsyncExtensions
    {
        private static readonly ILoggingService _logger;

#pragma warning disable S3963 // "static" fields should be initialized inline
        static ObjectCacheAsyncExtensions()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<T> GetCacheObjectAsync<T>(this IDistributedCache cache, string key, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            try
            {
                string value = await cache.GetStringAsync(key, token);
                if (!value.HasValue())
                {
                    return default;
                }
                return value.ToDeserialize<T>(jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default;
        }

        public static async Task SetCacheObjectAsync<T>(this IDistributedCache cache, string key, T value, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetCacheStringAsync(key, result, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetCacheObjectAsync<T>(this IDistributedCache cache, string key, T value, int minutes, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetCacheStringAsync(key, result, minutes, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetCacheObjectAsync(this IDistributedCache cache, string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetCacheStringAsync(key, result, time, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

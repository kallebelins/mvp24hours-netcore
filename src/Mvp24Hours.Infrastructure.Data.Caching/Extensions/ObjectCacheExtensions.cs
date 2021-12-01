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

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class ObjectCacheExtensions
    {
        private static readonly ILoggingService _logger;

#pragma warning disable S3963 // "static" fields should be initialized inline
        static ObjectCacheExtensions()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static T GetCacheObject<T>(this IDistributedCache cache, string key, JsonSerializerSettings jsonSerializerSettings = null)
        {
            try
            {
                string value = cache.GetString(key);
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

        public static void SetCacheObject<T>(this IDistributedCache cache, string key, T value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                cache.SetCacheString(key, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetCacheObject<T>(this IDistributedCache cache, string key, T value, int minutes, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                cache.SetCacheString(key, result, minutes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetCacheObject(this IDistributedCache cache, string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                cache.SetCacheString(key, result, time);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

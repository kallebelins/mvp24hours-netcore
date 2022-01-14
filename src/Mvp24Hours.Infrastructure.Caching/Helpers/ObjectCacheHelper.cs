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

namespace Mvp24Hours.Helpers
{
    public static class ObjectCacheHelper
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
        static ObjectCacheHelper()
        {
            _logger = ServiceProviderHelper.GetService<ILoggingService>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static T GetObject<T>(string key, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (Cache == null)
            {
                return default;
            }

            try
            {
                return Cache.GetCacheObject<T>(key, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default;
        }

        public static void SetObject<T>(string key, T value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheObject<T>(key, value, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetObject(string key, object value, int minutes, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheObject(key, value, minutes, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void SetObject(string key, object value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (Cache == null)
            {
                return;
            }

            try
            {
                Cache.SetCacheObject(key, value, time, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}

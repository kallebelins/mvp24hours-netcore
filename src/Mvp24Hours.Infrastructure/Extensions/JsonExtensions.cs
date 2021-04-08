//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string ToSerialize<T>(this T value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (value == null) return string.Empty;
            return JsonHelper.Serialize(value, jsonSerializerSettings);
        }

        public static T ToDeserialize<T>(this string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (!value.HasValue()) return default;
            return JsonHelper.Deserialize<T>(value, jsonSerializerSettings);
        }

        public static IPagingResult<T> ToDeserializePagingResult<T>(this string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<IPagingResult<T>>(value, JsonHelper.JsonPagingResultSettings<T>(jsonSerializerSettings));
        }

        public static IBusinessResult<T> ToDeserializeBusinessResult<T>(this string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<IBusinessResult<T>>(value, JsonHelper.JsonBusinessResultSettings<T>(jsonSerializerSettings));
        }

        public static dynamic ToDynamic<T>(this T obj)
        {
            return ObjectHelper.ConvertToDynamic(obj);
        }

        public static bool IsValidJson(this string src)
        {
            try
            {
                var asToken = JToken.Parse(src);
                return asToken.Type == JTokenType.Object || asToken.Type == JTokenType.Array;
            }
            catch (Exception)  // Typically a JsonReaderException exception if you want to specify.
            {
                return false;
            }
        }
    }
}

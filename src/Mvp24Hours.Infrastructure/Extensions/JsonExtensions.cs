using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string ToSerialize<T>(this T value)
        {
            if (value == null) return string.Empty;
            return JsonHelper.Serialize(value);
        }

        public static T ToDeserialize<T>(this string value)
        {
            if (!value.HasValue()) return default;
            return JsonHelper.Deserialize<T>(value);
        }

        public static IPagingResult<T> ToPagingResult<T>(this string value)
        {
            return JsonConvert.DeserializeObject<IPagingResult<T>>(value, JsonHelper.JsonPagingResultSettings<T>());
        }

        public static IBusinessResult<T> ToBusinessResult<T>(this string value)
        {
            return JsonConvert.DeserializeObject<IBusinessResult<T>>(value, JsonHelper.JsonBusinessResultSettings<T>());
        }
    }
}

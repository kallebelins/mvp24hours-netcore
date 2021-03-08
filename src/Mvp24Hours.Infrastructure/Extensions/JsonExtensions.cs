using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
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

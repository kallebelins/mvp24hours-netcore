using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        public static string Serialize<T>(T dto)
        {
            return JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static JsonSerializerSettings JsonPagingResultSettings<T>()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueObjectConverter<IPagingResult<T>, PagingResult<T>>());
            settings.Converters.Add(new ValueObjectConverter<IPageResult, PageResult>());
            settings.Converters.Add(new ValueObjectConverter<ISummaryResult, SummaryResult>());
            settings.Converters.Add(new ValueObjectConverter<ILinkResult, LinkResult>());
            settings.Converters.Add(new ValueObjectConverter<IMessageResult, MessageResult>());
            return settings;
        }

        public static JsonSerializerSettings JsonBusinessResultSettings<T>()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueObjectConverter<IBusinessResult<T>, BusinessResult<T>>());
            settings.Converters.Add(new ValueObjectConverter<ISummaryResult, SummaryResult>());
            settings.Converters.Add(new ValueObjectConverter<ILinkResult, LinkResult>());
            settings.Converters.Add(new ValueObjectConverter<IMessageResult, MessageResult>());
            return settings;
        }
    }
}

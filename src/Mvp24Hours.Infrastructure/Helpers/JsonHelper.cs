using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Converters;
using Mvp24Hours.Core.ValueObjects.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonDefaultSettings;

        static JsonHelper()
        {
            _jsonDefaultSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static JsonSerializerSettings JsonDefaultSettings
        {
            get
            {
                return _jsonDefaultSettings;
            }
            set
            {
                if (value != null)
                {
                    _jsonDefaultSettings = value;
                }
            }
        }


        public static string Serialize<T>(T dto, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return JsonConvert.SerializeObject(dto, jsonSerializerSettings ?? JsonDefaultSettings);
        }

        public static T Deserialize<T>(string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings ?? JsonDefaultSettings);
        }

        public static JsonSerializerSettings JsonPagingResultSettings<T>(JsonSerializerSettings jsonSerializerSettings = null)
        {
            var settings = jsonSerializerSettings ?? JsonDefaultSettings;
            settings.Converters.Add(new ValueObjectConverter<IPagingResult<T>, PagingResult<T>>());
            settings.Converters.Add(new ValueObjectConverter<IPageResult, PageResult>());
            settings.Converters.Add(new ValueObjectConverter<ISummaryResult, SummaryResult>());
            settings.Converters.Add(new ValueObjectConverter<ILinkResult, LinkResult>());
            settings.Converters.Add(new ValueObjectConverter<IMessageResult, MessageResult>());
            return settings;
        }

        public static JsonSerializerSettings JsonBusinessResultSettings<T>(JsonSerializerSettings jsonSerializerSettings = null)
        {
            var settings = jsonSerializerSettings ?? JsonDefaultSettings;
            settings.Converters.Add(new ValueObjectConverter<IBusinessResult<T>, BusinessResult<T>>());
            settings.Converters.Add(new ValueObjectConverter<ISummaryResult, SummaryResult>());
            settings.Converters.Add(new ValueObjectConverter<ILinkResult, LinkResult>());
            settings.Converters.Add(new ValueObjectConverter<IMessageResult, MessageResult>());
            return settings;
        }
    }
}

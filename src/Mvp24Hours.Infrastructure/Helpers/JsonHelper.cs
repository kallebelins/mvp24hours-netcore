using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class JsonHelper
    {
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

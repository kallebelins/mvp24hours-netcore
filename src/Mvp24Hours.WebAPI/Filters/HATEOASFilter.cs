//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using Microsoft.AspNetCore.Mvc.Filters;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mvp24Hours.WebAPI.Filters
{
    public class HATEOASFilter : IAsyncResultFilter
    {
        private readonly ILoggingService _logger;

        private readonly IHATEOASContext _hateaosContext;
        public static bool IsLoaded { get; private set; }
        public static bool EnableFilter { get; private set; }

        public HATEOASFilter(IHATEOASContext hateaosContext)
        {
            _logger = LoggingService.GetLoggingService();
            if (!IsLoaded)
            {
                string configEnableFilter = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:EnableHATEOAS");
                EnableFilter = !configEnableFilter.HasValue() || (bool)configEnableFilter.ToBoolean();
                IsLoaded = true;
            }
            _hateaosContext = hateaosContext;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!EnableFilter)
            {
                await next();
                return;
            }

            var originBody = context.HttpContext.Response.Body;

            try
            {
                using var newBody = new MemoryStream();

                context.HttpContext.Response.Body = newBody;

                await next();

                context.HttpContext.Response.Body = new MemoryStream();

                newBody.Seek(0, SeekOrigin.Begin);

                string newContent = new StreamReader(newBody).ReadToEnd();

                string result = GetNewContent(newContent);

                var memoryStreamModified = new MemoryStream();
                var sw = new StreamWriter(memoryStreamModified);
                sw.Write(result);
                sw.Flush();
                memoryStreamModified.Position = 0;

                await memoryStreamModified.CopyToAsync(originBody).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                context.HttpContext.Response.Body = originBody;
            }
        }

        private string GetNewContent(string newContent)
        {
            dynamic result = JObject.Parse(newContent);
            result.links = JArray.Parse(_hateaosContext.Links.ToSerialize());
            return ((object)result).ToSerialize();
        }
    }
}

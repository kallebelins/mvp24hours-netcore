//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.WebAPI.Filters
{
    public class HATEOASFilter : IAsyncResultFilter
    {
        private readonly IHATEOASContext _hateaosContext;
        public static bool IsLoaded { get; private set; }
        public static bool EnableFilter { get; private set; }

        public HATEOASFilter(IHATEOASContext hateaosContext)
        {
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

            if (_hateaosContext.HasLinks)
            {
                var value = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
                if (value != null)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    var result = value.ToDynamic();
                    result.links = _hateaosContext.Links;
                    await context.HttpContext.Response.WriteAsync(((object)result).ToSerialize());
                }
            }

            await next();
        }
    }
}

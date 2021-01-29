using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Mvp24Hours.WebAPI.Filters
{
    public class CustomSwaggerFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var nonMobileRoutes = swaggerDoc.Paths
                .Where(x => !x.Key.ToLower().Contains("public"))
                .ToList();
            nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }
    }
}

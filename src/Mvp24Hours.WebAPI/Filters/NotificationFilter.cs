using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace Mvp24Hours.WebAPI.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly INotificationContext _notificationContext;

        public NotificationFilter(INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notificationContext.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    HasErrors = true,
                    Errors = _notificationContext.Notifications
                });

                await context.HttpContext.Response.WriteAsync(result);

                return;
            }

            await next();
        }
    }
}

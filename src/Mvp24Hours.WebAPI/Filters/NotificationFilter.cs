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
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Mvp24Hours.WebAPI.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly INotificationContext _notificationContext;
        public static bool IsLoaded { get; private set; }
        public static bool EnableFilter { get; private set; }

        public NotificationFilter(INotificationContext notificationContext)
        {
            if (!IsLoaded)
            {
                string configEnableFilter = ConfigurationHelper.GetSettings("Mvp24Hours:Filters:EnableNotification");
                EnableFilter = !configEnableFilter.HasValue() || (bool)configEnableFilter.ToBoolean();
                IsLoaded = true;
            }
            _notificationContext = notificationContext;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!EnableFilter)
            {
                await next();
                return;
            }

            if (_notificationContext.HasNotifications)
            {
                if (_notificationContext.Notifications.Any(x => x.Type == Core.Enums.MessageType.Error))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.HttpContext.Response.ContentType = "application/json";
                    var boResult = new BusinessResult<Notification>(messages: _notificationContext.Notifications);
                    var result = JsonHelper.Serialize(boResult);
                    await context.HttpContext.Response.WriteAsync(result);
                }
                else
                {
                    var value = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
                    if (value != null)
                    {
                        context.HttpContext.Response.ContentType = "application/json";

                        var messages = new List<MessageResult>();
                        foreach (var item in _notificationContext.Notifications)
                        {
                            messages.Add(new MessageResult(item.Key, item.Message, item.Type));
                        }

                        PropertyInfo propInfo = value.GetType().GetProperty("Messages");
                        if (propInfo.GetValue(value, null) is IReadOnlyCollection<IMessageResult> valueMessages)
                        {
                            foreach (var item in valueMessages)
                            {
                                messages.Add(new MessageResult(item.Key, item.Message, item.Type));
                            }
                        }

                        var result = value.ToDynamic();
                        result.messages = messages;
                        await context.HttpContext.Response.WriteAsync(((object)result).ToSerialize());
                    }
                }
            }

            await next();
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Mvc.Filters;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Mvp24Hours.WebAPI.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly ILoggingService _logger;
        private readonly INotificationContext _notificationContext;
        public static bool IsLoaded { get; private set; }
        public static bool EnableFilter { get; private set; }

        public NotificationFilter(INotificationContext notificationContext)
        {
            _logger = LoggingService.GetLoggingService();
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
                var originBody = context.HttpContext.Response.Body;

                try
                {
                    using var newBody = new MemoryStream();

                    context.HttpContext.Response.Body = newBody;

                    await next();

                    context.HttpContext.Response.Body = new MemoryStream();

                    newBody.Seek(0, SeekOrigin.Begin);

                    context.HttpContext.Response.ContentType = "application/json";

                    string result;

                    if (_notificationContext.Notifications.Any(x => x.Type == Core.Enums.MessageType.Error))
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = NewContentError();
                    }
                    else
                    {
                        string newContent = new StreamReader(newBody).ReadToEnd();
                        result = NewContentMessage(context, newContent);
                    }

                    if (result.HasValue())
                    {
                        var memoryStreamModified = new MemoryStream();
                        var sw = new StreamWriter(memoryStreamModified);
                        sw.Write(result);
                        sw.Flush();
                        memoryStreamModified.Position = 0;
                        await memoryStreamModified.CopyToAsync(originBody).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
                finally
                {
                    context.HttpContext.Response.Body = originBody;
                }

                return;
            }

            await next();
        }

        private string NewContentMessage(ResultExecutingContext context, string currentContent)
        {
            var value = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
            if (value != null)
            {
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

                dynamic result = JObject.Parse(currentContent);
                result.messages = JArray.Parse(messages.ToSerialize());
                return ((object)result).ToSerialize();
            }

            return currentContent;
        }

        private string NewContentError()
        {
            var boResult = new BusinessResult<Notification>(messages: _notificationContext.Notifications);
            var result = JsonHelper.Serialize(boResult);
            return result;
        }
    }
}

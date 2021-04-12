//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;
using Mvp24Hours.Core.DTOs;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggingService _logger;
        private readonly bool TraceMiddleware;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _logger = LoggingService.GetLoggingService();
            _next = next;
            TraceMiddleware = GetTraceMiddleware();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Response.HasStarted)
                    await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (!httpContext.Response.HasStarted)
                    await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message;

            if (TraceMiddleware)
                message = $"Message: {(exception?.InnerException ?? exception).Message} / Trace: {exception.StackTrace}";
            else
                message = $"Message: {(exception?.InnerException ?? exception).Message}";

            var boResult = BusinessResult<VoidResult>.Create(
                message.ToMessageResult("internalservererror", MessageType.Error)
            );

            var messageResult = JsonHelper.Serialize(boResult);
            return context.Response.WriteAsync(messageResult);
        }

        private bool GetTraceMiddleware()
        {
            string enableTraceStr = ConfigurationHelper.GetSettings("Mvp24Hours:Web:TraceMiddleware");
            bool.TryParse(enableTraceStr, out bool enableTrace);
            return enableTrace;
        }
    }
}

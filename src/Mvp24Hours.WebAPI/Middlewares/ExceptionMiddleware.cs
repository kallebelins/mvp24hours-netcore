//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.DTOs;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggingService _logger;
        private readonly bool _traceMiddleware;

        public ExceptionMiddleware(RequestDelegate next, ILoggingService logger)
        {
            _next = next;
            _logger = logger;
            _traceMiddleware = ConfigurationHelper.GetSettings<bool>("Mvp24Hours:Web:TraceMiddleware");
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Response.HasStarted)
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (!httpContext.Response.HasStarted)
                {
                    await HandleExceptionAsync(httpContext, ex);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message;

            if (_traceMiddleware)
            {
                message = $"Message: {(exception?.InnerException ?? exception).Message} / Trace: {exception.StackTrace}";
            }
            else
            {
                message = $"Message: {(exception?.InnerException ?? exception).Message}";
            }

            var boResult = message
                .ToMessageResult("internalservererror", MessageType.Error)
                .ToBusiness<VoidResult>();

            var messageResult = JsonHelper.Serialize(boResult);
            return context.Response.WriteAsync(messageResult);
        }
    }
}

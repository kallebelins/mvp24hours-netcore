﻿using System;
using System.Net;
using System.Net.Http;

namespace Mvp24Hours.Core.Exceptions
{
    [Serializable]
    public class HttpStatusCodeException : HttpRequestException
    {
        public HttpStatusCodeException(HttpStatusCode statusCode, HttpMethod method = null, Uri requestUri = null)
            : this(null, statusCode, method, requestUri)
        {
        }

        public HttpStatusCodeException(string message, HttpStatusCode statusCode, HttpMethod method = null, Uri requestUri = null, string responseBody = null)
                   : base(message ?? $"Non-success HTTP status code: {(int)statusCode} {statusCode}.")
        {
            StatusCode = statusCode;
            Method = method;
            RequestUri = requestUri;
            ResponseBody = responseBody;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public HttpMethod Method { get; private set; }
        public Uri RequestUri { get; private set; }
        public string ResponseBody { get; set; }
    }
}

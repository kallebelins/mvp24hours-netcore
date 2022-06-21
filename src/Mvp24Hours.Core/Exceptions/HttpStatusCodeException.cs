using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Mvp24Hours.Core.Exceptions
{
    [Serializable]
    public class HttpStatusCodeException : HttpRequestException
    {
        public HttpStatusCodeException(HttpStatusCode statusCode, HttpMethod method = null, Uri requestUri = null)
            : this(null, statusCode, method, requestUri)
        {
        }

        public HttpStatusCodeException(string message, HttpStatusCode statusCode, HttpMethod method = null, Uri requestUri = null)
            : base(message ?? $"Non-success HTTP status code: {(int)statusCode} {statusCode}.")
        {
            StatusCode = statusCode;
            Method = method;
            RequestUri = requestUri;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public HttpMethod Method { get; private set; }
        public Uri RequestUri { get; private set; }
    }
}

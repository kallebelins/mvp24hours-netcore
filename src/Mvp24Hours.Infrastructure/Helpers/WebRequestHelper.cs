//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// Contains functions for web requests
    /// </summary>
    public static class WebRequestHelper
    {
        private static readonly ILoggingService _logger;

        static WebRequestHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }

        /// <summary>
        /// 
        /// </summary>
        public static Encoding EncodingRequest { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        public static string ToQueryString(params object[] objs)
        {
            if (objs == null)
            {
                return string.Empty;
            }

            var result = new List<string>();
            foreach (var obj in objs)
            {
                var props = obj
                    .GetType()
                    .GetProperties()
                    .Where(p => p.GetValue(obj, null) != null);

                foreach (var p in props)
                {
                    var value = p.GetValue(obj, null);
                    if (value is ICollection enumerable)
                    {
                        result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                    }
                    else
                    {
                        result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                    }
                }
            }

            return string.Join("&", result.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<string> PostAsync(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null)
        {
            return await SendAsync(urlService, header, credentials, "POST", data);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<T> PostAsync<T>(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            var result = await SendAsync(urlService, header, credentials, "POST", data);
            if (!result.HasValue())
            {
                return default;
            }

            return result.ToDeserialize<T>(jsonSerializerSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<string> GetAsync(string url, Hashtable header = null, ICredentials credentials = null)
        {
            return await SendAsync(url, header, credentials, "GET", null);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<T> GetAsync<T>(string url, Hashtable header = null, ICredentials credentials = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            var result = await SendAsync(url, header, credentials, "GET", null);
            if (!result.HasValue())
            {
                return default;
            }

            return result.ToDeserialize<T>(jsonSerializerSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<string> PutAsync(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null)
        {
            return await SendAsync(urlService, header, credentials, "PUT", data);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<T> PutAsync<T>(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            var result = await SendAsync(urlService, header, credentials, "PUT", data);
            if (!result.HasValue())
            {
                return default;
            }

            return result.ToDeserialize<T>(jsonSerializerSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<string> DeleteAsync(string url, Hashtable header = null, ICredentials credentials = null)
        {
            return await SendAsync(url, header, credentials, "DELETE", null);
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<T> DeleteAsync<T>(string url, Hashtable header = null, ICredentials credentials = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            var result = await SendAsync(url, header, credentials, "DELETE", null);
            if (!result.HasValue())
            {
                return default;
            }

            return result.ToDeserialize<T>(jsonSerializerSettings);
        }

        private static async Task<string> SendAsync(string url, Hashtable header, ICredentials credentials, string method, string data)
        {
            string result = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (EncodingRequest == null)
                {
                    EncodingRequest = Encoding.UTF8;
                }

                HttpWebRequest requisicao = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                requisicao.Method = method;
                requisicao.ContentType = "application/json; charset=utf-8";
                requisicao.Headers.Add("Accept-Encoding", "gzip,deflate");
                if (credentials != null)
                {
                    requisicao.Credentials = credentials;
                }

                requisicao.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (header != null)
                {
                    foreach (DictionaryEntry hash in header)
                    {
                        if (hash.Key.ToString() == "ContentType")
                        {
                            requisicao.ContentType = hash.Value.ToString();
                            continue;
                        }
                        requisicao.Headers.Add(hash.Key.ToString(), hash.Value.ToString());
                    }
                }
                requisicao.Timeout = 300000;
                bool hasData = (method == "POST" || method == "PUT");
                byte[] bytes = null;
                if (hasData)
                {
                    if (data == null)
                    {
                        data = "";
                    }

                    string postData = data;
                    bytes = EncodingRequest.GetBytes(postData);
                    requisicao.ContentLength = bytes.Length;
                }

                try
                {
                    if (!hasData)
                    {
                        using var response = requisicao.GetResponse();
                        using var content = response.GetResponseStream();
                        using var reader = new StreamReader(content, EncodingRequest);
                        result = await reader.ReadToEndAsync();
                    }
                    else
                    {
                        using var reqstream = requisicao.GetRequestStream();
                        reqstream.Write(bytes, 0, bytes.Length);
                        var httpResponse = (HttpWebResponse)requisicao.GetResponse();
                        using var streamReader = new StreamReader(httpResponse.GetResponseStream(), EncodingRequest);
                        result = await streamReader.ReadToEndAsync();
                    }
                }
                catch (WebException we)
                {
                    _logger.Error(we);
                    if (we.Response != null)
                    {
                        using var stream = we.Response.GetResponseStream();
                        using var reader = new StreamReader(stream);
                        return reader.ReadToEnd();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            return result;
        }

    }
}

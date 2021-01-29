//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Infrastructure.Log;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class ServiceRequestHelper
    {
        private static readonly ILoggingService _logger;

        static ServiceRequestHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }

        public static Encoding Codificacao { get; set; } = Encoding.UTF8;

        public async static Task<string> PostAsync(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null)
        {
            return await sendAsync(urlService, header, credentials, "POST", data);
        }
        public async static Task<string> GetAsync(string url, Hashtable header = null, ICredentials credentials = null)
        {
            return await sendAsync(url, header, credentials, "GET", null);
        }

        public async static Task<string> PutAsync(string urlService, string data = "", Hashtable header = null, ICredentials credentials = null)
        {
            return await sendAsync(urlService, header, credentials, "PUT", data);
        }

        public async static Task<string> DeleteAsync(string url, Hashtable header = null, ICredentials credentials = null)
        {
            return await sendAsync(url, header, credentials, "DELETE", null);
        }

        private async static Task<string> sendAsync(string url, Hashtable header, ICredentials credentials, string method, string data)
        {
            string result = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (Codificacao == null)
                    Codificacao = Encoding.UTF8;
                HttpWebRequest requisicao = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                requisicao.Method = method;
                requisicao.ContentType = "application/json; charset=utf-8";
                requisicao.Headers.Add("Accept-Encoding", "gzip,deflate");
                if (credentials != null)
                    requisicao.Credentials = credentials;
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
                bool hasData = (method == "POST" || method == "PATCH");
                byte[] bytes = null;
                if (hasData)
                {
                    if (data == null)
                        data = "";
                    string postData = data;
                    bytes = Codificacao.GetBytes(postData);
                    requisicao.ContentLength = bytes.Length;
                }

                try
                {
                    if (!hasData)
                    {
                        using (var response = requisicao.GetResponse())
                        {
                            using (var content = response.GetResponseStream())
                            {
                                using (var reader = new StreamReader(content, Codificacao))
                                {
                                    result = await reader.ReadToEndAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var reqstream = requisicao.GetRequestStream())
                        {
                            reqstream.Write(bytes, 0, bytes.Length);
                            var httpResponse = (HttpWebResponse)requisicao.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Codificacao))
                            {
                                result = await streamReader.ReadToEndAsync();
                            }
                        }
                    }
                }
                catch (WebException we)
                {
                    if (we.Response != null)
                    {
                        using (var stream = we.Response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                    else throw;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return result;
        }

    }
}

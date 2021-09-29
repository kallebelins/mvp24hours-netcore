using System;
using System.Net;
using System.ServiceModel;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class ServiceRequestHelper
    {
        public static TClient Client<TClient>(string url)
            where TClient : class
        {
            if (url.StartsWith("https"))
            {
                return ClientHttps<TClient>(url);
            }
            else
            {
                return ClientHttp<TClient>(url);
            }
        }

        public static TClient ClientHttps<TClient>(string url)
            where TClient : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;

            var endpoint = new EndpointAddress(new Uri(url));
            return (TClient)Activator.CreateInstance(typeof(TClient), binding, endpoint);
        }

        public static TClient ClientHttp<TClient>(string url)
            where TClient : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;

            var endpoint = new EndpointAddress(new Uri(url));
            return (TClient)Activator.CreateInstance(typeof(TClient), binding, endpoint);
        }
    }
}

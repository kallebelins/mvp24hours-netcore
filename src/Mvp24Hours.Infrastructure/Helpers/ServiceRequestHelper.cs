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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

            // 229d9aa00eda75238409390d0dedc62367f7d8d1

            var endpoint = new EndpointAddress(new Uri(url));
            return (TClient)Activator.CreateInstance(typeof(TClient), binding, endpoint);
        }
    }
}

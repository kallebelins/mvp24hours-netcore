using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.RabbitMQ
{
    public class RabbitMQConfiguration : BaseVO
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HostName;
            yield return Port;
            yield return UserName;
            yield return Password;
        }
    }
}

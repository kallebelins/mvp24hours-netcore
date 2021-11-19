using Mvp24Hours.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ.ValueObjects
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

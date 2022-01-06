using Mvp24Hours.Application.RabbitMQ.Test.Support.Common;
using Mvp24Hours.Infrastructure.RabbitMQ;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers
{
    public class CustomerProducer : MvpRabbitMQProducer
    {
        public CustomerProducer()
            : base(EventBusConstants.CustomerQueue) { }
    }
}

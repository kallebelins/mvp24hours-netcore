using Microsoft.Extensions.Options;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Infrastructure.RabbitMQ;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers
{
    public class CustomerProducer : MvpRabbitMQProducer<CustomerEvent>
    {
        public CustomerProducer(IOptions<RabbitMQOptions> options)
            : base(options)
        {
        }
    }
}

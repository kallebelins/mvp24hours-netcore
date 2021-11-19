using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQConsumer<in T>
        where T : class
    {
        Task Received(T message);
    }
}

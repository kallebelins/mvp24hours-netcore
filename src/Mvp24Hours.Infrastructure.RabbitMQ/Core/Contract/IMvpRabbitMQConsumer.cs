using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQConsumer<in T>
        where T : class
    {
        void Consume();
        Task Received(T message);
    }
}

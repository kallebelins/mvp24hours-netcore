namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQProducer
    {
        public void Publish<T>(T message) where T : class;
        public void Publish(string message);
    }
}

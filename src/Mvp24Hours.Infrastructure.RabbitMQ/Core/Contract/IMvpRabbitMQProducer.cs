//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Enums;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQProducer<in T>
        where T : class
    {
        public void Publish(T message, MvpRabbitMQPriorityEnum priorityEnum = MvpRabbitMQPriorityEnum.Normal);
        public void Publish(string message, MvpRabbitMQPriorityEnum priorityEnum = MvpRabbitMQPriorityEnum.Normal);
    }
}

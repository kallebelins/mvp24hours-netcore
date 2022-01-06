using System;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Dto
{
    public abstract class MessageBusEvent
    {
        public MessageBusEvent()
        {
            Token = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }

        public MessageBusEvent(Guid token, DateTime createDate)
        {
            Token = token;
            Created = createDate;
        }

        public Guid Token { get; private set; }

        public DateTime Created { get; private set; }
    }
}

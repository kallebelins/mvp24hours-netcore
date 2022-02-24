//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Enums;
using RabbitMQ.Client.Events;
using System;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQEventHook
    {
        void Execute(string eventName, string message);
        void Execute(string eventName, IBusinessEvent businessEvent, BasicDeliverEventArgs deliverEvent = null);
        void Execute(string eventName, Exception exception);
    }
}

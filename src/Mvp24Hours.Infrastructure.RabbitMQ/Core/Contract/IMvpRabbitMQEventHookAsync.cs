//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQEventHookAsync
    {
        Task ExecuteAsync(string eventName, string message);
        Task ExecuteAsync(string eventName, IBusinessEvent businessEvent, BasicDeliverEventArgs deliverEvent = null);
        Task ExecuteAsync(string eventName, Exception exception);
    }
}

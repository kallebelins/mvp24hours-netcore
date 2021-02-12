using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class MessageResultExtensions
    {
        public static IEnumerable<IMessageResult> ToMessageResult(this IEnumerable<string> messages, MessageType messageType)
        {
            var messagesResult = new List<IMessageResult>();
            foreach (var item in messages)
                messagesResult.Add(new MessageResult(item ?? "Undefined message.", messageType));
            return messagesResult;
        }

        public static IMessageResult ToMessageResult(this string message, string key, MessageType messageType)
        {
            return new MessageResult(key, message ?? "Undefined message.", messageType);
        }

        public static IMessageResult ToMessageResult(this string message, MessageType messageType)
        {
            return new MessageResult(message ?? "Undefined message.", messageType);
        }
    }
}

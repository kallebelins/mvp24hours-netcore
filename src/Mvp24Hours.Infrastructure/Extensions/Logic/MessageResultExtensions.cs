//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageResultExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<IMessageResult> ToMessageResult(this IEnumerable<string> messages, MessageType messageType)
        {
            var messagesResult = new List<IMessageResult>();
            foreach (var item in messages)
            {
                messagesResult.Add(new MessageResult(item ?? "Undefined message.", messageType));
            }

            return messagesResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMessageResult ToMessageResult(this string message, string key, MessageType messageType)
        {
            return new MessageResult(key, message ?? "Undefined message.", messageType);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMessageResult ToMessageResult(this string message, MessageType messageType)
        {
            return new MessageResult(message ?? "Undefined message.", messageType);
        }
    }
}

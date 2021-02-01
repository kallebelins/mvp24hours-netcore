//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.Enums;
using System;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult"/>
    /// </summary>
    public class MessageResult : IMessageResult
    {
        public MessageResult(string message, MessageType messageType)
            : this(Guid.NewGuid().ToString(), message, messageType)
        {
        }

        public MessageResult(string key, string message, MessageType messageType)
        {
            Key = key;
            Message = message;
            Type = messageType;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Key"/>
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Message"/>
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Type"/>
        /// </summary>
        public MessageType Type { get; }
    }
}

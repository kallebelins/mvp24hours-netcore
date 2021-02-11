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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class MessageResult : BaseVO, IMessageResult
    {
        #region [ Ctor ]
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
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Key"/>
        /// </summary>
        [DataMember]
        public string Key { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Message"/>
        /// </summary>
        [DataMember]
        public string Message { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult.Type"/>
        /// </summary>
        [DataMember]
        public MessageType Type { get; }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Key;
            yield return Message;
            yield return Type;
        }
        #endregion
    }
}

//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    /// <summary>
    /// Represents a notification
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class Notification : BaseVO, IMessageResult
    {
        #region [ Ctor ]
        public Notification(string message, int typeCode)
            : this(null, message, typeCode)
        {
        }

        public Notification(string message, MessageType type)
            : this(null, message, type)
        {
        }

        [JsonConstructor]
        public Notification(string key, string message, int typeCode)
        {
            Key = key;
            Message = message;
            Type = (MessageType)typeCode;
        }

        public Notification(string key, string message, MessageType type = MessageType.Error)
        {
            Key = key;
            Message = message;
            Type = type;
        }
        #endregion

        #region [ Fields / Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult.Key"/>
        /// </summary>
        [DataMember]
        public string Key { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult.Message"/>
        /// </summary>
        [DataMember]
        public string Message { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult.Type"/>
        /// </summary>
        [DataMember]
        public MessageType Type { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult.TypeCode"/>
        /// </summary>
        [DataMember]
        public int TypeCode { get { return (int)Type; } }

        #endregion

        #region [ Overrides ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.ValueObjects.BaseVO.GetEqualityComponents"/>
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Key;
        }
        #endregion
    }
}

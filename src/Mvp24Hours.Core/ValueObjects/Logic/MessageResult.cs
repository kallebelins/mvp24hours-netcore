//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Mvp24Hours.Core.Converters;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IMessageResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class MessageResult : BaseVO, IMessageResult
    {
        #region [ Ctor ]
        public MessageResult(string message, MessageType type)
            : this(Guid.NewGuid().ToString(), message, type)
        {
        }

        [JsonConstructor]
        public MessageResult(string key, string message, int typeCode)
        {
            Key = key;
            Message = message;
            Type = (MessageType)typeCode;
        }

        public MessageResult(string key, string message, MessageType type)
        {
            Key = key;
            Message = message;
            Type = type;
        }
        #endregion

        #region [ Properties ]
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

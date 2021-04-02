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
        [JsonConstructor]
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
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; }
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

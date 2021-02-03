//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.Enums;
using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    /// <summary>
    /// Represents a notification
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IMessageResult"/>
    /// </summary>
    public class Notification : BaseVO, IMessageResult
    {
        #region [ Ctor ]
        public Notification(string key, string message)
            : this(key, message, MessageType.Error)
        {
        }
        public Notification(string key, string message, MessageType type)
        {
            Key = key;
            Message = message;
            Type = type;
        }
        #endregion

        #region [ Fields / Properties ]
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

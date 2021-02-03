//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class BusinessResult<T> : IBusinessResult<T>
    {
        #region [ Ctor ]

        public BusinessResult()
        {
        }

        public BusinessResult(IList<T> data)
            : this()
        {
            this.data = data;
        }

        #endregion

        #region [ Properties ]

        IList<T> data;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Data"/>
        /// </summary>
        [DataMember]
        public IList<T> Data
        {
            get
            {
                return data ?? (data = new List<T>());
            }
        }

        private IList<IMessageResult> _messages;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Messages"/>
        /// </summary>
        [DataMember]
        public IList<IMessageResult> Messages
        {
            get
            {
                return _messages ?? (_messages = new List<IMessageResult>());
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.HasErrors"/>
        /// </summary>
        [DataMember]
        public bool HasErrors => Messages.Where(x => x.Type == Enums.MessageType.Error).Any();

        IList<ILinkResult> links;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Links"/>
        /// </summary>
        [DataMember]
        public IList<ILinkResult> Links
        {
            get
            {
                return links ?? (links = new List<ILinkResult>());
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Token"/>
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        #endregion
    }
}

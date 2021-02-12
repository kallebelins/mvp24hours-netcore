//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class BusinessResult<T> : BaseVO, IBusinessResult<T>
    {
        #region [ Ctor ]

        public BusinessResult(
            IReadOnlyCollection<T> data = null,
            IReadOnlyCollection<IMessageResult> messages = null,
            IList<ILinkResult> links = null,
            string token = null
        )
        {
            Data = data;
            Messages = messages;
            Links = links;
            Token = token;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Data"/>
        /// </summary>
        [DataMember]
        public IReadOnlyCollection<T> Data { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Messages"/>
        /// </summary>
        [DataMember]
        public IReadOnlyCollection<IMessageResult> Messages { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.HasErrors"/>
        /// </summary>
        [DataMember]
        public bool HasErrors => Messages?.Where(x => x.Type == Enums.MessageType.Error)?.Any() ?? false;

        private IList<ILinkResult> links;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Links"/>
        /// </summary>
        [DataMember]
        public IList<ILinkResult> Links
        {
            get
            {
                return links ??= new List<ILinkResult>();
            }
            private set
            {
                links = value;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IBusinessResult{T}.Token"/>
        /// </summary>
        [DataMember]
        public string Token { get; }

        #endregion

        #region [ Methods ]

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
            yield return Data;
            yield return Messages;
            yield return Links;
        }

        #endregion

        #region [ Static's ]
        public static IBusinessResult<T> Create(params IMessageResult[] messageResult)
        {
            return new BusinessResult<T>(
                messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
            );
        }
        public static IBusinessResult<T> Create(T value, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new BusinessResult<T>(
                    data: new ReadOnlyCollection<T>(new List<T> { value }),
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new BusinessResult<T>();
        }
        #endregion
    }
}

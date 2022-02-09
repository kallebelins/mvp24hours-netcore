//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingResult{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingResult<T> : BusinessResult<T>, IPagingResult<T>
    {
        #region [ Ctor ]

        public PagingResult(
            IPageResult paging,
            ISummaryResult summary,
            T data = default,
            IReadOnlyCollection<IMessageResult> messages = null,
            string token = null
        ) : base(data, messages, token)
        {
            Paging = paging;
            Summary = summary;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingResult{T}.Paging"/>
        /// </summary>
        [DataMember]
        public IPageResult Paging { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingResult{T}.Summary"/>
        /// </summary>
        [DataMember]
        public ISummaryResult Summary { get; }

        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Paging;
            yield return Summary;
            yield return base.GetEqualityComponents();
        }
        #endregion
    }
}

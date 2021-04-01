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
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingResult{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingResult<T> : BusinessResult<T>, IPagingResult<T>
    {
        #region [ Ctor ]

        public PagingResult(
            IPageResult paging,
            ISummaryResult summary,
            IReadOnlyCollection<T> data = null,
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

        #region [ Static's ]

        public static new IPagingResult<T> Create(params IMessageResult[] messageResult)
        {
            return Create(null, messageResult);
        }

        public static new IPagingResult<T> Create(string tokenDefault, params IMessageResult[] messageResult)
        {
            return new PagingResult<T>(
                new PageResult(0, 0, 0),
                new SummaryResult(0, 0),
                token: tokenDefault,
                messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
            );
        }

        public static new IPagingResult<T> Create(T value, params IMessageResult[] messageResult)
        {
            return Create(value, null, messageResult);
        }

        public static new IPagingResult<T> Create(T value, string tokenDefault, params IMessageResult[] messageResult)
        {
            if (value != null)
            {
                return new PagingResult<T>(
                    new PageResult(0, 0, 0),
                    new SummaryResult(0, 0),
                    token: tokenDefault,
                    data: new ReadOnlyCollection<T>(new List<T> { value }),
                    messages: new ReadOnlyCollection<IMessageResult>(messageResult?.ToList() ?? new List<IMessageResult>())
                );
            }
            return new PagingResult<T>(
                new PageResult(0, 0, 0),
                new SummaryResult(0, 0)
            );
        }
        #endregion

    }
}

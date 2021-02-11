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
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPageResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class PageResult : BaseVO, IPageResult
    {
        #region [ Ctor ]
        public PageResult(int limit, int offset, int count)
        {
            Limit = limit;
            Offset = offset;
            Count = count;
        }
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPageResult.Limit"/>
        /// </summary>
        [DataMember]
        public int Limit { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPageResult.Offset"/>
        /// </summary>
        [DataMember]
        public int Offset { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPageResult.Count"/>
        /// </summary>
        [DataMember]
        public int Count { get; }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Limit;
            yield return Offset;
            yield return Count;
        }
        #endregion
    }
}

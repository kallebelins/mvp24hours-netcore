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
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingCriteria"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingCriteriaExpression<T> : PagingCriteria, IPagingCriteriaExpression<T>
    {
        #region [ Ctor ]
        public PagingCriteriaExpression(
            int limit,
            int offset,
            IReadOnlyCollection<string> orderBy = null,
            IReadOnlyCollection<string> navigation = null)
            : base(limit, offset, orderBy, navigation)
        {
        }
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingCriteria.OrderByAscendingExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IReadOnlyCollection<Expression<Func<T, dynamic>>> OrderByAscendingExpr { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingCriteria.OrderByDescendingExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IReadOnlyCollection<Expression<Func<T, dynamic>>> OrderByDescendingExpr { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.IPagingCriteria.NavigationExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IReadOnlyCollection<Expression<Func<T, dynamic>>> NavigationExpr { get; }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return OrderByAscendingExpr;
            yield return OrderByDescendingExpr;
            yield return NavigationExpr;
        }
        #endregion
    }
}

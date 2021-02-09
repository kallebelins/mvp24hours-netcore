//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingCriteria<T> : IPagingCriteria<T>
    {
        #region [ Ctor ]
        public PagingCriteria(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
        }
        #endregion

        #region [ Fields ]
        private IList<Expression<Func<T, dynamic>>> orderByAscendingExpr;
        private IList<Expression<Func<T, dynamic>>> orderByDescendingExpr;
        private IList<string> orderBy;
        private IList<Expression<Func<T, dynamic>>> navigationExpr;
        private IList<string> navigation;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.Limit"/>
        /// </summary>
        [DataMember]
        public int Limit { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.Offset"/>
        /// </summary>
        [DataMember]
        public int Offset { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.OrderByAscendingExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> OrderByAscendingExpr
        {
            get
            {
                return orderByAscendingExpr ??= new List<Expression<Func<T, dynamic>>>();
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.OrderByDescendingExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> OrderByDescendingExpr
        {
            get
            {
                return orderByDescendingExpr ??= new List<Expression<Func<T, dynamic>>>();
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.OrderBy"/>
        /// </summary>
        [DataMember]
        public IList<string> OrderBy
        {
            get
            {
                return orderBy ??= new List<string>();
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.NavigationExpr"/>
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> NavigationExpr
        {
            get
            {
                return navigationExpr ??= new List<Expression<Func<T, dynamic>>>();
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria{T}.Navigation"/>
        /// </summary>
        [DataMember]
        public IList<string> Navigation
        {
            get
            {
                return navigation ??= new List<string>();
            }
        }
        #endregion
    }
}

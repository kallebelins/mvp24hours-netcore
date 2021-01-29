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
    /// Class representing query clauses (pagination, sorting, navigation)
    /// </summary>
    [DataContract, Serializable]
    public class PagingCriteria<T> : IPagingCriteria<T>
    {
        public PagingCriteria()
        {
            OrderByAscendingExpr = new List<Expression<Func<T, dynamic>>>();
            OrderByDescendingExpr = new List<Expression<Func<T, dynamic>>>();
            OrderBy = new List<string>();
            NavigationExpr = new List<Expression<Func<T, dynamic>>>();
            Navigation = new List<string>();
        }

        /// <summary>
        /// Number of items per page
        /// </summary>
        [DataMember]
        public int Limit { get; set; }

        /// <summary>
        /// Page Number (Block Limited)
        /// </summary>
        [DataMember]
        public int Offset { get; set; }

        /// <summary>
        /// Sort criteria ascending expression
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> OrderByAscendingExpr { get; set; }

        /// <summary>
        /// Sort criteria descending expression
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> OrderByDescendingExpr { get; set; }

        /// <summary>
        /// Sort criteria with "name ASC" or "name DESC"
        /// </summary>
        [DataMember]
        public IList<string> OrderBy { get; set; }

        /// <summary>
        /// Criteria for objects loaded along with the object consulted
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Expression<Func<T, dynamic>>> NavigationExpr { get; set; }

        /// <summary>
        /// Criteria for objects loaded along with the object consulted
        /// </summary>
        [DataMember]
        public IList<string> Navigation { get; set; }
    }
}

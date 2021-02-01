using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    /// <summary>
    /// Represents a definition for search criteria on a page
    /// </summary>
    public interface IPagingCriteria<T>
    {
        /// <summary>
        /// Limit items on the page
        /// </summary>
        int Limit { get; set; }
        /// <summary>
        /// Item block number or page number
        /// </summary>
        int Offset { get; set; }
        /// <summary>
        /// Clause for sorting by field
        /// </summary>
        IList<string> OrderBy { get; set; }
        /// <summary>
        /// Expression for sorting by ascending field
        /// </summary>
        IList<Expression<Func<T, dynamic>>> OrderByAscendingExpr { get; set; }
        /// <summary>
        /// Expression for sorting by descending field
        /// </summary>
        IList<Expression<Func<T, dynamic>>> OrderByDescendingExpr { get; set; }
        /// <summary>
        /// Related objects that will be loaded together
        /// </summary>
        IList<string> Navigation { get; set; }
        /// <summary>
        /// Expression for loading related objects
        /// </summary>
        IList<Expression<Func<T, dynamic>>> NavigationExpr { get; set; }
    }
}

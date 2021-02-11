using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    /// <summary>
    /// Represents a definition for search criteria on a page
    /// </summary>
    public interface IPagingCriteria
    {
        /// <summary>
        /// Limit items on the page
        /// </summary>
        int Limit { get; }
        /// <summary>
        /// Item block number or page number
        /// </summary>
        int Offset { get; }
        /// <summary>
        /// Clause for sorting by field
        /// </summary>
        IList<string> OrderBy { get; }
        /// <summary>
        /// Related objects that will be loaded together
        /// </summary>
        IList<string> Navigation { get; }
    }
}

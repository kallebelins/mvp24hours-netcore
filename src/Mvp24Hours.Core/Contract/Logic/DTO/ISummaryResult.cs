namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    /// <summary>
    /// Represents a pagination summary
    /// </summary>
    public interface ISummaryResult
    {
        /// <summary>
        /// Total number of items
        /// </summary>
        int TotalCount { get; set; }
        /// <summary>
        /// Total number of pages or item groups
        /// </summary>
        int TotalPages { get; set; }
    }
}

namespace Mvp24Hours.Core.Contract.ValueObjects.Logic
{
    /// <summary>
    /// Represents a pagination summary
    /// </summary>
    public interface ISummaryResult
    {
        /// <summary>
        /// Total number of items
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Total number of pages or item groups
        /// </summary>
        int TotalPages { get; }
    }
}

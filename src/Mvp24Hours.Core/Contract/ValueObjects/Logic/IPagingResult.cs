namespace Mvp24Hours.Core.Contract.ValueObjects.Logic
{
    /// <summary>
    /// Paging business object used to encapsulate responses
    /// </summary>
    public interface IPagingResult<T> : IBusinessResult<T>
    {
        /// <summary>
        /// Pagination details
        /// </summary>
        IPageResult Paging { get; }
        /// <summary>
        /// Pagination summary
        /// </summary>
        ISummaryResult Summary { get; }
    }
}

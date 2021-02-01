namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    /// <summary>
    /// Paging business object used to encapsulate responses
    /// </summary>
    public interface IPagingResult<T> : IBusinessResult<T>
    {
        /// <summary>
        /// Pagination details
        /// </summary>
        IPageResult Paging { get; set; }
        /// <summary>
        /// Pagination summary
        /// </summary>
        ISummaryResult Summary { get; set; }
    }
}

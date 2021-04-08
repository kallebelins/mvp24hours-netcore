//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
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

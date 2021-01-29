namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    public interface IPagingResult<T> : IBusinessResult<T>
    {
        IPageResult Paging { get; set; }
        ISummaryResult Summary { get; set; }
    }
}

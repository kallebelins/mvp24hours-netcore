namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    public interface IPageResult
    {
        int Limit { get; set; }
        int Offset { get; set; }
        int Count { get; set; }
    }
}

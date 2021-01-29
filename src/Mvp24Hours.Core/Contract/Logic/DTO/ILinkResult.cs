namespace Mvp24Hours.Core.Contract.Logic
{
    public interface ILinkResult
    {
        string Href { get; set; }
        string Rel { get; set; }
        string Method { get; set; }
        bool? IsTemplate { get; set; }
    }
}

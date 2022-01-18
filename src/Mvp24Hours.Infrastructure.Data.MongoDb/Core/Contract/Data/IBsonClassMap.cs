namespace Mvp24Hours.Core.Contract.Data
{
    public interface IBsonClassMap<TSource>
        where TSource : class
    {
        void Configure();
    }
}

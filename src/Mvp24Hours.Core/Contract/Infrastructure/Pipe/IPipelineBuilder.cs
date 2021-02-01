namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Strategic for aggregating operations in the pipeline
    /// </summary>
    /// <example>
    /// <code>
    /// // interface
    /// public interface IProductCategoryListBuilder : IPipelineBuilder { }
    /// 
    /// // implementation
    /// public class ProductCategoryListBuilder : IProductCategoryListBuilder
    /// {
    ///     public IPipelineAsync Builder(IPipelineAsync pipeline)
    ///     {
    ///         return pipeline
    ///             .AddAsync(new ProductCategoryFileOperation())
    ///             .AddAsync(new ProductCategoryResponseMapperOperation());
    ///     }
    /// }
    /// 
    /// // use
    /// var builder = HttpContextHelper.GetService{IProductGetByBuilder}();
    /// builder.Builder(pipeline);
    /// </code>
    /// </example>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Operations aggregator
        /// </summary>
        IPipelineAsync Builder(IPipelineAsync pipeline);
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Core.Contract.Application.Pipe
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
    ///             .Add<ProductCategoryFileOperation>()
    ///             .Add<ProductCategoryResponseMapperOperation>();
    ///     }
    /// }
    /// 
    /// // use
    /// var builder = ServiceProviderHelper.GetService{IProductGetByBuilder}();
    /// builder.Builder(pipeline);
    /// </code>
    /// </example>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Operations aggregator
        /// </summary>
        IPipeline Builder(IPipeline pipeline);
    }
}

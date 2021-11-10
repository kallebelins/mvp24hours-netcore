//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine
    /// </summary>
    /// <example>
    /// <code>
    ///     IPipeline pipeline = new Pipeline();
    ///     pipeline.Add(new FileLogWriteOperation());
    ///     var result = await pipeline.Execute(filter.ToMessage());
    ///     return result.ToBusiness{Product}();
    /// </code>
    /// </example>
    public interface IPipeline
    {
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add(IOperation operation);
        /// <summary>
        /// Records operations
        /// </summary>
        IPipeline Add<T>() where T : IOperation;
        /// <summary>
        /// Performs operations
        /// </summary>
        IPipelineMessage Execute(IPipelineMessage input);
    }
}

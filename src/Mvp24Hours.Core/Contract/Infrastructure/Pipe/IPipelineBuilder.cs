namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    public interface IPipelineBuilder
    {
        IPipelineAsync Builder(IPipelineAsync pipeline);
    }
}

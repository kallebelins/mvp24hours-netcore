namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    public interface IOperationMapperAsync<T> : IOperationAsync
    {
        T Mapper(params object[] data);
    }
}

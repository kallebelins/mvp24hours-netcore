namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    public interface IOperationMapper<T> : IOperation
    {
        T Mapper(params object[] data);
    }
}

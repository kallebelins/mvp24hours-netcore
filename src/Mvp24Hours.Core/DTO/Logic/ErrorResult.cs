using Mvp24Hours.Core.Contract.Logic.DTO;

namespace Mvp24Hours.Core.DTO.Logic
{
    public class ErrorResult : IErrorResult
    {
        public ErrorResult(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; }
        public string Message { get; }

    }
}

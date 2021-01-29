using Mvp24Hours.Core.Contract.Logic.DTO;
using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    public class Notification : BaseVO, IErrorResult
    {
        public Notification(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; }
        public string Message { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Key;
        }
    }
}

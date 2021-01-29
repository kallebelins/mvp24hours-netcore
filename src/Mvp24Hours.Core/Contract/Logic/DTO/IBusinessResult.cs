using Mvp24Hours.Core.Contract.Logic.DTO;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Logic
{
    public interface IBusinessResult<T>
    {
        IList<T> Data { get; }

        IList<IErrorResult> Errors { get; }

        bool HasErrors { get; }

        IList<ILinkResult> Links { get; }

        string Token { get; set; }
    }
}

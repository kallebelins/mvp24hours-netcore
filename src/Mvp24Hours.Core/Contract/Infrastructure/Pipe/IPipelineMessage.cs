//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Business object used to traffic data
    /// </summary>
    public interface IPipelineMessage
    {
        bool IsLocked { get; }
        bool IsSucess { get; set; }
        IList<string> Errors { get; }
        string Token { get; set; }

        void AddContent<T>(T obj);

        T GetContent<T>();

        IList<object> GetContentAll();

        void Lock();
    }
}

//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine base
    /// </summary>
    public abstract class PipelineBase
    {
        #region [ Ctor ]
        protected PipelineBase(bool isBreakOnFail = false)
        {
            this.IsBreakOnFail = isBreakOnFail;
        }
        #endregion

        #region [ Fields / Properties ]
        protected readonly bool IsBreakOnFail;
        protected IPipelineMessage Message { get; set; }
        #endregion

        #region [ Methods ]

        public IPipelineMessage GetMessage() => Message;

        #endregion
    }
}

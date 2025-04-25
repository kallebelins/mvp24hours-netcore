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
        protected PipelineBase()
            : this(false, false, false)
        {
        }

        protected PipelineBase(bool isBreakOnFail, bool forceRollbackOnFalure)
            : this(isBreakOnFail, forceRollbackOnFalure, false)
        {
        }

        protected PipelineBase(bool isBreakOnFail, bool forceRollbackOnFalure, bool allowPropagateException)
        {
            IsBreakOnFail = isBreakOnFail;
            ForceRollbackOnFalure = forceRollbackOnFalure;
            AllowPropagateException = allowPropagateException; 
        }
        #endregion

        #region [ Fields / Properties ]
        protected bool IsBreakOnFail { get; set; }
        public bool AllowPropagateException { get; set; }
        public bool ForceRollbackOnFalure { get; set; }
        protected IPipelineMessage Message { get; set; }
        #endregion

        #region [ Methods ]

        public IPipelineMessage GetMessage() => Message;

        #endregion
    }
}

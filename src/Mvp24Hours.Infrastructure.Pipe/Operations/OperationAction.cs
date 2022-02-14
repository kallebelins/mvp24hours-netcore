//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System;

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Action operation
    /// </summary>
    public class OperationAction : IOperation
    {
        private readonly Action<IPipelineMessage> _action;
        private readonly bool _isRequired;

        public virtual bool IsRequired => this._isRequired;

        public OperationAction(Action<IPipelineMessage> action, bool isRequired = false)
        {
            this._action = action;
            this._isRequired = isRequired;
        }

        public virtual void Execute(IPipelineMessage input)
        {
            this._action?.Invoke(input);
        }
    }
}

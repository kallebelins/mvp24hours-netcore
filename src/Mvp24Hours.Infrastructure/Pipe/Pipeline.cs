//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Pipe
{
    public class Pipeline : IPipeline
    {
        private List<IOperation> operations = new List<IOperation>();
        private bool _isBreakOnFail;
        private string _token;

        public Pipeline()
            : this(true)
        {

        }

        public Pipeline(string token)
            : this(token, true)
        {
        }

        public Pipeline(bool isBreakOnFail)
            : this(Guid.NewGuid().ToString(), isBreakOnFail)
        {
        }

        public Pipeline(string token, bool isBreakOnFail)
        {
            this._isBreakOnFail = isBreakOnFail;
            this._token = token;
        }

        public IPipeline Add(IOperation operation)
        {
            this.operations.Add(operation);
            return this;
        }

        public IPipelineMessage Execute(IPipelineMessage input)
        {
            return this.operations.Aggregate(input, (current, operation) =>
            {
                current.Token = this._token;
                if (!operation.IsRequired && !current.IsSucess && this._isBreakOnFail)
                    return current;
                if (current.IsLocked)
                    return current;
                try
                {
                    return operation.Execute(current);
                }
                catch (Exception ex)
                {
                    current.IsSucess = false;
                    current.Errors.Add((ex?.InnerException ?? ex).Message);
                    input.AddContent(ex);
                }
                return current;
            });
        }
    }
}

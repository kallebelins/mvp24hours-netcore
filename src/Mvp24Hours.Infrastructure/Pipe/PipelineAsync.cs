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
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe
{
    public class PipelineAsync : IPipelineAsync
    {
        private List<IOperationAsync> _operations = new List<IOperationAsync>();
        private bool _isBreakOnFail;
        private string _token;

        public PipelineAsync()
            : this(true)
        {

        }

        public PipelineAsync(string token)
            : this(token, true)
        {
        }


        public PipelineAsync(bool isBreakOnFail)
            : this(Guid.NewGuid().ToString(), isBreakOnFail)
        {
        }

        public PipelineAsync(string token, bool isBreakOnFail)
        {
            this._isBreakOnFail = isBreakOnFail;
            this._token = token ?? Guid.NewGuid().ToString();
        }

        public IPipelineAsync AddAsync(IOperationAsync operation)
        {
            this._operations.Add(operation);
            return this;
        }

        public async Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            return await this._operations.Aggregate(Task.FromResult(input), async (current, operation) =>
            {
                var result = await current;
                result.Token = this._token;
                if (!operation.IsRequired && !result.IsSucess && this._isBreakOnFail)
                    return result;
                if (result.IsLocked)
                    return result;
                try
                {
                    return await operation.Execute(result);
                }
                catch (Exception ex)
                {
                    result.IsSucess = false;
                    result.Errors.Add((ex?.InnerException ?? ex).Message);
                    input.AddContent(ex);
                }
                return result;
            });
        }
    }
}

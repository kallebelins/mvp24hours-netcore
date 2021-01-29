//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Files
{
    public class FileTokenWriteOperation<T> : OperationBaseAsync
    {
        public override Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            var dto = input.GetContent<T>();

            if (dto == null)
                return Task.FromResult(input);

            FileLogHelper.WriteLogToken(input.Token, typeof(T).Name.ToLower(), dto);

            return Task.FromResult(input);
        }
    }
}

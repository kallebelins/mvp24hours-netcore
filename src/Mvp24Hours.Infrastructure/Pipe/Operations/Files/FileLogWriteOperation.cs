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
    public class FileLogWriteOperation : OperationBaseAsync
    {
        public override bool IsRequired => true;

        public override Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            FileLogHelper.WriteLog(input.GetContentAll(), "message", $"Token: {input.Token} / IsSuccess: {input.IsSucess} / Warnings: {string.Join('/', input.Errors)}");
            return Task.FromResult(input);
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Files
{
    /// <summary>
    /// Log writing operation
    /// </summary>
    public class FileLogWriteOperation : OperationBaseAsync
    {
        public override bool IsRequired => true;
        public virtual string FileLogPath => null;

        public override Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            FileLogHelper.WriteLog(input.GetContentAll(), "message", $"Token: {input.Token} / IsSuccess: {input.IsSuccess} / Warnings: {string.Join('/', input.Messages)}", FileLogPath);
            return Task.FromResult(input);
        }
    }
}

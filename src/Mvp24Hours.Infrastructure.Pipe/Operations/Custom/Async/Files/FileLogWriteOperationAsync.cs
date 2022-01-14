//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom.Files
{
    /// <summary>
    /// Log writing operation
    /// </summary>
    public class FileLogWriteOperationAsync : OperationBaseAsync
    {
        static bool _enable = ConfigurationHelper.GetSettings<bool>("Mvp24Hours:Operation:FileLog:Enable");

        public override bool IsRequired => true;
        public virtual string FileLogPath => null;

        public override Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
        {
            if (_enable)
            {
                FileLogHelper.WriteLog(input.GetContentAll(), "message", $"Token: {input.Token} / IsSuccess: {input.IsFaulty} / Warnings: {string.Join('/', input.Messages)}", FileLogPath);
            }
            return Task.FromResult(input);
        }
    }
}

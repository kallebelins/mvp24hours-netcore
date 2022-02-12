//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom.Files
{
    /// <summary>
    /// Log writing operation
    /// </summary>
    public class FileLogWriteOperationAsync : OperationBaseAsync
    {
        public override bool IsRequired => true;
        private readonly string filePath;
        public virtual string FilePath => filePath;

        public FileLogWriteOperationAsync(string _filePath)
        {
            this.filePath = _filePath;
        }

        public FileLogWriteOperationAsync(INotificationContext _notificationContext, string _filePath)
            : base(_notificationContext)
        {
            this.filePath = _filePath;
        }

        public override Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
        {
            if (FilePath.HasValue())
            {
                FileLogHelper.WriteLog(input.GetContentAll(), "message", $"Token: {input.Token} / IsSuccess: {input.IsFaulty} / Warnings: {string.Join('/', input.Messages)}", FilePath);
            }
            return Task.FromResult(input);
        }
    }
}

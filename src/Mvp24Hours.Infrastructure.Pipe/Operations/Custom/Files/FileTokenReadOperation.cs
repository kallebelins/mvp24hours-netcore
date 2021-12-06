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

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom.Files
{
    /// <summary>
    /// Operation to read file log token
    /// </summary>
    public class FileTokenReadOperation<T> : OperationBaseAsync
    {
        public virtual string FileLogPath => null;

        public override Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
        {
            var dto = FileLogHelper.ReadLogToken<T>(input.Token, typeof(T).Name.ToLower(), FileLogPath);

            if (dto != null)
            {
                input.AddContent(dto);
            }

            return Task.FromResult(input);
        }
    }
}

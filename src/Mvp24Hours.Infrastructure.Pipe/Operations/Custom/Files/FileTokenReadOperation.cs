//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom.Files
{
    /// <summary>
    /// Operation to read file log token
    /// </summary>
    public class FileTokenReadOperation<T> : OperationBase
    {
        static readonly bool _enable = ConfigurationHelper.GetSettings<bool>("Mvp24Hours:Operation:FileToken:Enable");

        public virtual string FileLogPath => null;

        public override IPipelineMessage Execute(IPipelineMessage input)
        {
            if (_enable)
            {
                var dto = FileLogHelper.ReadLogToken<T>(input.Token, typeof(T).Name.ToLower(), FileLogPath);
                if (dto != null)
                {
                    input.AddContent(dto);
                }
            }
            return input;
        }
    }
}

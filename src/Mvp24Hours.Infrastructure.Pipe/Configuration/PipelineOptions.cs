//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Infrastructure.Pipe.Configuration
{
    public sealed class PipelineOptions
    {
        public bool IsBreakOnFail { get; set; }

        public IPipeline Pipeline => ServiceProviderHelper.GetService<IPipeline>();
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================

using Mvp24Hours.Core.Contract.Logic.DTO;

namespace Mvp24Hours.Core.DTO.Logic
{
    public class SummaryResult : ISummaryResult
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

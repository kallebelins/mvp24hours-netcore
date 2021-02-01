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
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult"/>
    /// </summary>
    public class SummaryResult : ISummaryResult
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult.TotalCount"/>
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult.TotalPages"/>
        /// </summary>
        public int TotalPages { get; set; }
    }
}

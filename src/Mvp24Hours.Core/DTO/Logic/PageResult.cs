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
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult"/>
    /// </summary>
    public class PageResult : IPageResult
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Limit"/>
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Offset"/>
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Count"/>
        /// </summary>
        public int Count { get; set; }
    }
}

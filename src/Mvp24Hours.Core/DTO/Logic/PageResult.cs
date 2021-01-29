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
    public class PageResult : IPageResult
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic;

namespace Mvp24Hours.Core.DTO.Logic
{
    public class LinkResult : ILinkResult
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
        public bool? IsTemplate { get; set; }
    }
}

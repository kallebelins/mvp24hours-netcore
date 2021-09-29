//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Contexts
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Contexts.IHATEOASContext"/>
    /// </summary>
    public class HATEOASContext : IHATEOASContext
    {
        #region [ Fields / Properties ]
        private readonly List<ILinkResult> _links;
        public IReadOnlyCollection<ILinkResult> Links => _links;
        public bool HasLinks => _links.Any();
        #endregion

        #region [ Ctor ]
        public HATEOASContext()
        {
            _links = new List<ILinkResult>();
        }
        #endregion

        #region [ Methods ]
        public void AddLink(string href, string rel, string method)
        {
            _links.Add(new LinkResult(href, rel, method));
        }
        public void AddLink(string href, string rel, string method, bool? isTemplate)
        {
            _links.Add(new LinkResult(href, rel, method, isTemplate));
        }
        public void AddLink(ILinkResult link)
        {
            _links.Add(link);
        }
        public void AddLinks(IEnumerable<ILinkResult> links)
        {
            _links.AddRange(links);
        }
        #endregion
    }
}

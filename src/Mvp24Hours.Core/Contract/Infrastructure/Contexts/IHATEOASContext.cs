//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Infrastructure.Contexts
{
    /// <summary>
    /// Context that represents a container for in-app links Hateoas
    /// </summary>
    public interface IHateoasContext
    {
        /// <summary>
        /// List of links
        /// </summary>
        IReadOnlyCollection<ILinkResult> Links { get; }
        /// <summary>
        /// Indicates whether there is link in the context
        /// </summary>
        bool HasLinks { get; }
        /// <summary>
        /// Adds a link to the context
        /// </summary>
        void AddLink(string href, string rel, string method);
        /// <summary>
        /// Adds a link to the context
        /// </summary>
        void AddLink(string href, string rel, string method, bool? isTemplate);
        /// <summary>
        /// Adds a link to the context
        /// </summary>
        void AddLink(ILinkResult link);
        /// <summary>
        /// Adds a list of links to the context
        /// </summary>
        void AddLinks(IEnumerable<ILinkResult> links);
    }
}

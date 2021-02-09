namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Defines the information needed to access a resource
    /// </summary>
    public interface ILinkResult
    {
        /// <summary>
        /// Resource URI
        /// </summary>
        string Href { get; }
        /// <summary>
        /// Describes how the URI relates to the current resource
        /// </summary>
        string Rel { get; }
        /// <summary>
        /// Method for web request
        /// </summary>
        string Method { get; }
        /// <summary>
        /// Indicates whether the URI is a template or definitive address
        /// </summary>
        bool? IsTemplate { get; }
    }
}

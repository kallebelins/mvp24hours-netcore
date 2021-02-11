using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.ValueObjects.Logic
{
    /// <summary>
    /// Business object used to encapsulate response to requests
    /// </summary>
    public interface IBusinessResult<T>
    {
        /// <summary>
        /// Encapsulated model data list
        /// </summary>
        IReadOnlyCollection<T> Data { get; }
        /// <summary>
        /// Business messages for user feedback
        /// </summary>
        IReadOnlyCollection<IMessageResult> Messages { get; }
        /// <summary>
        /// Indicates if you have an error message
        /// </summary>
        bool HasErrors { get; }
        /// <summary>
        /// Indicates the paths that the client can follow without knowing the API completely (HATEOAS)
        /// </summary>
        IList<ILinkResult> Links { get; }
        /// <summary>
        /// Transaction reference token
        /// </summary>
        string Token { get; }
    }
}

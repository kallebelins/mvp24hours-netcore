//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Infrastructure.Pipe
{
    /// <summary>
    /// Business object used to traffic data through the pipeline
    /// </summary>
    public interface IPipelineMessage
    {
        #region [ Properties ]
        /// <summary>
        /// Indicates whether the message is blocked
        /// </summary>
        bool IsLocked { get; }
        /// <summary>
        /// Indicates whether the message is successfully traveling through the pipeline
        /// </summary>
        bool IsSuccess { get; }
        /// <summary>
        /// List of feedback messages
        /// </summary>
        IList<IMessageResult> Messages { get; }
        /// <summary>
        /// Transaction reference token
        /// </summary>
        string Token { get; }
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Adds content to be attached to the message by type
        /// </summary>
        void AddContent<T>(T obj);
        /// <summary>
        /// Adds content to be attached to the message by key
        /// </summary>
        void AddContent<T>(string key, T obj);
        /// <summary>
        /// Get content by type
        /// </summary>
        T GetContent<T>();
        /// <summary>
        /// Get content by key
        /// </summary>
        T GetContent<T>(string key);
        /// <summary>
        /// Checks for content by type
        /// </summary>
        bool HasContent<T>();
        /// <summary>
        /// Checks for content by key
        /// </summary>
        bool HasContent(string key);
        /// <summary>
        /// Get all content
        /// </summary>
        IList<object> GetContentAll();
        /// <summary>
        /// Blocks message for non-mandatory operations
        /// </summary>
        void SetLock();
        /// <summary>
        /// Defines token content
        /// </summary>
        void SetToken(string token);
        /// <summary>
        /// Defines whether an operation failed or broke the message
        /// </summary>
        void SetFailure();
        #endregion
    }
}

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
        /// <summary>
        /// Indicates whether the message is blocked
        /// </summary>
        bool IsLocked { get; }
        /// <summary>
        /// Indicates whether the message is successfully traveling through the pipeline
        /// </summary>
        bool IsSuccess { get; set; }
        /// <summary>
        /// List of feedback messages
        /// </summary>
        IList<IMessageResult> Messages { get; }
        /// <summary>
        /// Transaction reference token
        /// </summary>
        string Token { get; set; }
        /// <summary>
        /// Adds content to be attached to the message by type
        /// </summary>
        void AddContent<T>(T obj);
        /// <summary>
        /// Get content by type
        /// </summary>
        T GetContent<T>();
        /// <summary>
        /// Get all content
        /// </summary>
        IList<object> GetContentAll();
        /// <summary>
        /// Blocks message for non-mandatory operations
        /// </summary>
        void Lock();
    }
}

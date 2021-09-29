//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Enums;

namespace Mvp24Hours.Core.Contract.ValueObjects.Logic
{
    /// <summary>
    /// Reply message template
    /// </summary>
    public interface IMessageResult
    {
        /// <summary>
        /// Reference key
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Message to user
        /// </summary>
        string Message { get; }
        /// <summary>
        /// User feedback type
        /// </summary>
        MessageType Type { get; }
        /// <summary>
        /// User feedback type code
        /// </summary>
        int TypeCode { get; }
    }
}

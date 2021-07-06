//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

namespace Mvp24Hours.Core.Contract.Domain.Entity
{
    /// <summary>
    /// In terms of programing language, An entity can be any container class that has few properties with unique Id on it. Where Id represents the uniqueness of the entity class.
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// Represents the entity's unique identifier
        /// </summary>
        object EntityKey { get; }
    }
}

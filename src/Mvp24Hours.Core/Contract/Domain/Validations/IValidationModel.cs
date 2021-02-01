//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.Domain.Validations
{
    /// <summary>
    /// Used to validate model (entity, valueobjects, dto, ...)
    /// </summary>
    public interface IValidationModel
    {
        /// <summary>
        /// Ensures the model is valid
        /// </summary>
        /// <returns>true|false</returns>
        bool IsValid();
    }
}

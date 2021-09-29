namespace Mvp24Hours.Core.Contract.Domain.Validations
{
    /// <summary>
    /// Specification testing
    /// </summary>
    public interface IValidator<T>
    {
        /// <summary>
        /// Tests whether the model meets all added specifications
        /// </summary>
        /// <param name="Candidate">Model object for testing</param>
        /// <returns>true|false</returns>
        bool Validate(T Candidate);
    }
}

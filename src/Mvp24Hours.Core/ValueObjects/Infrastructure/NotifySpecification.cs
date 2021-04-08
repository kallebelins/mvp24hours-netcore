//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Specifications;
using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    /// <summary>
    /// Represents a notification with specification
    /// </summary>
    public class NotifySpecification<T> : BaseVO
    {
        #region [ Ctor ]

        public NotifySpecification(ISpecification<T> specification)
        {
            Specification = specification;
        }

        public NotifySpecification(string keyValidation, string messageValidation, ISpecification<T> specification)
            : this(specification)
        {
            KeyValidation = keyValidation;
            MessageValidation = messageValidation;
        }

        #endregion

        #region [ Fields / Properties ]

        /// <summary>
        /// Reference key for validation message
        /// </summary>
        public string KeyValidation { get; }
        /// <summary>
        /// Validation message for specification not met
        /// </summary>
        public string MessageValidation { get; }
        /// <summary>
        /// Test specification
        /// </summary>
        public ISpecification<T> Specification { get; }

        #endregion

        #region [ Overrides ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.ValueObjects.BaseVO.GetEqualityComponents"/>
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return KeyValidation;
        }
        #endregion
    }
}

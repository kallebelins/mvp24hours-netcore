//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Core.ValueObjects
{
    /// <summary>
    /// Base value object
    /// </summary>
    public abstract class BaseVO : IValidationModel<BaseVO>
    {
        #region [ Equality ]
        /// <summary>
        /// Gets value that defines the object instance
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();
        #endregion

        #region [ Overrides ]

        /// <summary>
        /// <see cref="System.Object.Equals(object?)"/>
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var valueObject = (BaseVO)obj;
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }
        /// <summary>
        /// <see cref="System.Object.GetHashCode"/>
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    return HashCode.Combine(current, obj);
                });
        }
        /// <summary>
        /// Equality comparator
        /// </summary>
        public static bool operator ==(BaseVO a, BaseVO b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }
        /// <summary>
        /// Inequality comparator
        /// </summary>
        public static bool operator !=(BaseVO a, BaseVO b)
        {
            return !(a == b);
        }

        #endregion

        #region [ Valid ]

        /// <summary>
        /// Checks whether the model meets specifications (default true)
        /// </summary>
        /// <returns>true|false</returns>
        public virtual bool IsValid()
        {
            return true;
        }

        public virtual bool IsValid(IValidatorNotify<BaseVO> validatorNotify)
        {
            return IsValid();
        }

        #endregion
    }
}

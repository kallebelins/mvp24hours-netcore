//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Core.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseVO : IValidationModel
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        #region [ Overrides ]

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                throw new ArgumentException($"Invalid comparison of Value Objects of different types: {GetType()} and {obj.GetType()}");
            var valueObject = (BaseVO)obj;
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    return HashCode.Combine(current, obj);
                });
        }

        public static bool operator ==(BaseVO a, BaseVO b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(BaseVO a, BaseVO b)
        {
            return !(a == b);
        }

        #endregion

        #region [ Valid ]

        protected ISpecificationModel<BaseVO> ValidSpecification = null;

        public bool IsValid()
        {
            return ValidSpecification?.IsSatisfiedBy(this) ?? true;
        }

        #endregion
    }
}

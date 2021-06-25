//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Infrastructure.Helpers;
using System;

namespace Mvp24Hours.Infrastructure.Validations
{
    public class ValidatorEntityNotify<T> : ValidatorNotify<T>, IValidatorNotify<T>
        where T : class, IEntityBase
    {
        #region [ Props ]
        private IUnitOfWork unitOfWork = null;
        protected virtual IUnitOfWork UnitOfWork
        {
            get { return (unitOfWork ??= ServiceProviderHelper.GetService<IUnitOfWork>()); }
        }
        #endregion

        #region [ Ctor ]
        public ValidatorEntityNotify()
            : base()
        {
        }
        #endregion

        #region [ Validate ]
        public override bool Validate(T Candidate)
        {
            foreach (var item in NotifySpecifications)
            {
                bool satisfiedBy = true;
                var specification = item.Specification;

                if (specification is ISpecificationQuery<T>)
                {
                    var specificationQuery = specification as ISpecificationQuery<T>;
                    satisfiedBy = UnitOfWork.GetRepository<T>().GetByAny(specificationQuery.IsSatisfiedByExpression);
                }
                else if (specification is ISpecificationModel<T>)
                {
                    var specificationModel = specification as ISpecificationModel<T>;
                    satisfiedBy = specificationModel.IsSatisfiedBy(Candidate);
                }

                if (!satisfiedBy)
                {
                    if (!string.IsNullOrEmpty(item.KeyValidation))
                    {
                        Context.Add(item.KeyValidation, item.MessageValidation);
                    }
                    else
                    {
                        Context.Add(Guid.NewGuid().ToString(), specification.ToString());
                    }
                }
            }
            return IsValid;
        }
        #endregion
    }
}

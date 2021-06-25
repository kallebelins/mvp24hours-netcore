//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Validations
{
    public class ValidatorNotify<T> : IValidatorNotify<T>
        where T : class
    {
        #region [ Ctor ]
        public ValidatorNotify()
        {
            Context = ServiceProviderHelper.GetService<INotificationContext>();

            if (Context == null)
            {
                throw new ArgumentNullException("Notification context is mandatory.");
            }

            NotifySpecifications = new List<NotifySpecification<T>>();
        }
        #endregion

        #region [ Props ]
        public INotificationContext Context { get; private set; }
        public bool IsValid => !Context.HasErrorNotifications;
        #endregion

        #region [ Specification ]

        public IValidatorNotify<T> AddSpecification<U>(string key, string message)
            where U : ISpecification<T>, new()
        {
            NotifySpecifications.Add(new NotifySpecification<T>(key, message, new U()));
            return this;
        }

        public IValidatorNotify<T> AddSpecification<U>()
            where U : ISpecification<T>, new()
        {
            var specification = new U();
            if (specification is ISpecificationValidator<T>)
            {
                var specificationValidator = specification as ISpecificationValidator<T>;
                NotifySpecifications.Add(new NotifySpecification<T>(specificationValidator.KeyValidation, specificationValidator.MessageValidation, specification));
            }
            else
            {
                NotifySpecifications.Add(new NotifySpecification<T>(specification));
            }
            return this;
        }

        public IValidatorNotify<T> AddSpecification<U>(ISpecification<T> specification)
            where U : ISpecification<T>
        {
            if (specification is ISpecificationValidator<T>)
            {
                var specificationValidator = specification as ISpecificationValidator<T>;
                NotifySpecifications.Add(new NotifySpecification<T>(specificationValidator.KeyValidation, specificationValidator.MessageValidation, specification));
            }
            else
            {
                NotifySpecifications.Add(new NotifySpecification<T>(specification));
            }
            return this;
        }

        public IValidatorNotify<T> AddSpecification<U>(ISpecificationValidator<T> specification)
            where U : ISpecificationValidator<T>
        {
            NotifySpecifications.Add(new NotifySpecification<T>(specification.KeyValidation, specification.MessageValidation, specification));
            return this;
        }

        #endregion

        #region [ Lists ]

        protected List<NotifySpecification<T>> NotifySpecifications { get; set; }

        #endregion

        #region [ Validate ]
        public virtual bool Validate(T Candidate)
        {
            foreach (var item in NotifySpecifications)
            {
                bool satisfiedBy = true;
                var specification = item.Specification;

                if (specification is ISpecificationModel<T>)
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
                        Context.Add(Guid.NewGuid().ToString(), item.Specification.ToString());
                    }
                }
            }
            return IsValid;
        }
        #endregion
    }
}

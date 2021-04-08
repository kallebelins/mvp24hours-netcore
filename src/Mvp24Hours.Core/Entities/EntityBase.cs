//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.Entities
{
    /// <summary>
    /// Represents an entity
    /// </summary>
    public abstract class EntityBase<T> : IEntityBase, IValidationModel
    {
        #region [ Primitive members ]

        /// <summary>
        /// Entity identifier used as a reference by the interface
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual object EntityKey => this.Id;
        /// <summary>
        /// Entity identifier
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public virtual T Id { get; set; }

        #endregion

        #region [ Valid ]

        /// <summary>
        /// Specification for entity
        /// </summary>
        protected ISpecificationModel<IEntityBase> ValidSpecification = null;
        /// <summary>
        /// Checks whether the entity meets the specification
        /// </summary>
        /// <returns>true|false</returns>
        public bool IsValid()
        {
            return ValidSpecification?.IsSatisfiedBy(this) ?? true;
        }

        #endregion
    }
}

using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.Entities
{
    public abstract class EntityBase<T> : IEntityBase, IValidationModel
    {
        #region [ Primitive members ]

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual object EntityKey => this.Id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public virtual T Id { get; set; }

        #endregion

        #region [ Valid ]

        protected ISpecificationModel<IEntityBase> ValidSpecification = null;

        public bool IsValid()
        {
            return ValidSpecification?.IsSatisfiedBy(this) ?? true;
        }

        #endregion
    }
}

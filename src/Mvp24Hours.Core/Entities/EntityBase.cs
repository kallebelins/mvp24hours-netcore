//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.Entities
{
    /// <summary>
    /// Represents an entity
    /// </summary>
    public abstract class EntityBase<TObject, TKey> : IEntityBase, IValidationModel<TObject>
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
        public virtual TKey Id { get; set; }

        #endregion

        #region [ Valid ]

        /// <summary>
        /// Checks whether the entity meets the specification (default true)
        /// </summary>
        /// <returns>true|false</returns>
        public virtual bool IsValid(IValidatorNotify<TObject> validatorNotify)
        {
            var results = new List<ValidationResult>();
            var contexto = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, contexto, results, true))
            {
                if (validatorNotify != null)
                {
                    foreach (var item in results)
                    {
                        validatorNotify.Context.Add(string.Join("|", item.MemberNames), item.ErrorMessage, Enums.MessageType.Error);
                    }
                }
                return false;
            }
            return true;
        }

        public bool IsValid()
        {
            return IsValid(null);
        }

        #endregion
    }
}

using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.Entities
{
    public abstract class EntityBaseLog<T, U> : EntityBase<T>, IEntityBase, IValidationModel, IEntityLog<U>
    {
        #region [ Log ]
        [DataMember]
        public DateTime Created { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public U CreatedBy { get; set; }
        [DataMember]
        public DateTime? Modified { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public U ModifiedBy { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public DateTime? Removed { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public U RemovedBy { get; set; }
        #endregion
    }
}

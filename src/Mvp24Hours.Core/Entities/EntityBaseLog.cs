//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Core.Entities
{
    /// <summary>
    /// Represents an entity with characteristics for logging (creation, modification and logical exclusion)
    /// </summary>
    /// <typeparam name="T">Represents entity</typeparam>
    /// <typeparam name="U">Represents data type used to log</typeparam>
    public abstract class EntityBaseLog<T, U> : EntityBase<T>, IEntityBase, IValidationModel<IEntityBase>, IEntityLog<U>
    {
        #region [ Log ]
        /// <summary>
        /// Creation date
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }
        /// <summary>
        /// Application or database user who created this record
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public U CreatedBy { get; set; }
        /// <summary>
        /// Modified date
        /// </summary>
        [DataMember]
        public DateTime? Modified { get; set; }
        /// <summary>
        /// Application or database user who modified this record
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public U ModifiedBy { get; set; }
        /// <summary>
        /// Logical exclusion date
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public DateTime? Removed { get; set; }
        /// <summary>
        /// Application or database user who logically deleted this record
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public U RemovedBy { get; set; }
        #endregion
    }
}

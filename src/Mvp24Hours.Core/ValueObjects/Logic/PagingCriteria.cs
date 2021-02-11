//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingCriteria : BaseVO, IPagingCriteria
    {
        #region [ Ctor ]
        public PagingCriteria(int limit, int offset, IList<string> orderBy = null, IList<string> navigation = null)
        {
            Limit = limit;
            Offset = offset;
            _orderBy = orderBy;
            _navigation = navigation;

        }
        #endregion

        #region [ Fields ]
        private IList<string> _orderBy;
        private IList<string> _navigation;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria.Limit"/>
        /// </summary>
        [DataMember]
        public int Limit { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria.Offset"/>
        /// </summary>
        [DataMember]
        public int Offset { get; }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria.OrderBy"/>
        /// </summary>
        [DataMember]
        public IList<string> OrderBy
        {
            get
            {
                return _orderBy ??= new List<string>();
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingCriteria.Navigation"/>
        /// </summary>
        [DataMember]
        public IList<string> Navigation
        {
            get
            {
                return _navigation ??= new List<string>();
            }
        }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Limit;
            yield return Offset;
            yield return _orderBy;
            yield return _navigation;
        }
        #endregion
    }
}

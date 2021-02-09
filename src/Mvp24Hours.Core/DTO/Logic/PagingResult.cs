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

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingResult{T}"/>
    /// </summary>
    [DataContract, Serializable]
    public class PagingResult<T> : BusinessResult<T>, IPagingResult<T>
    {
        #region [ Ctor ]

        public PagingResult(IPageResult paging, ISummaryResult summary, IList<T> data = null)
             : base(data)
        {
            Paging = paging;
            Summary = summary;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingResult{T}.Paging"/>
        /// </summary>
        [DataMember]
        public IPageResult Paging { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPagingResult{T}.Summary"/>
        /// </summary>
        [DataMember]
        public ISummaryResult Summary { get;  }

        #endregion
    }
}

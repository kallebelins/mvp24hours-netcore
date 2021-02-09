//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class SummaryResult : ISummaryResult
    {
        public SummaryResult(int totalCount, int totalPages)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult.TotalCount"/>
        /// </summary>
        [DataMember]
        public int TotalCount { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.ISummaryResult.TotalPages"/>
        /// </summary>
        [DataMember]
        public int TotalPages { get; }
    }
}

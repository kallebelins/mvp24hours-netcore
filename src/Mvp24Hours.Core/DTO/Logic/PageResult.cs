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
    /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class PageResult : IPageResult
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Limit"/>
        /// </summary>
        [DataMember]
        public int Limit { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Offset"/>
        /// </summary>
        [DataMember]
        public int Offset { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.DTO.IPageResult.Count"/>
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }
}

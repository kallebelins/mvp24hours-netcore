//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic;
using System;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class LinkResult : ILinkResult
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Href"/>
        /// </summary>
        [DataMember]
        public string Href { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Rel"/>
        /// </summary>
        [DataMember]
        public string Rel { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Method"/>
        /// </summary>
        [DataMember]
        public string Method { get; set; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.IsTemplate"/>
        /// </summary>
        [DataMember]
        public bool? IsTemplate { get; set; }
    }
}

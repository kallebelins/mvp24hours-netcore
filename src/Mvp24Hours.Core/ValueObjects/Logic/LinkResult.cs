//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class LinkResult : BaseVO, ILinkResult
    {
        #region [ Ctor ]
        public LinkResult(string href, string rel, string method, bool? isTemplate = null)
        {
            Href = href;
            Rel = rel;
            Method = method;
            IsTemplate = isTemplate;
        }
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Href"/>
        /// </summary>
        [DataMember]
        public string Href { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Rel"/>
        /// </summary>
        [DataMember]
        public string Rel { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.Method"/>
        /// </summary>
        [DataMember]
        public string Method { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ILinkResult.IsTemplate"/>
        /// </summary>
        [DataMember]
        public bool? IsTemplate { get; }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Href;
            yield return Rel;
            yield return Method;
            yield return IsTemplate;
        }
        #endregion
    }
}

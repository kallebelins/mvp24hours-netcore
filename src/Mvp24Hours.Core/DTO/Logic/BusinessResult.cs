//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.DTO.Logic
{
    /// <summary>
    /// Encapsulates result for data transfer
    /// </summary>
    [DataContract, Serializable]
    public class BusinessResult<T> : IBusinessResult<T>
    {
        #region [ Ctor ]

        public BusinessResult()
        {
        }

        public BusinessResult(IList<T> data)
            : this()
        {
            this.data = data;
        }

        #endregion

        #region [ Properties ]

        IList<T> data;
        public IList<T> Data
        {
            get
            {
                return data ?? (data = new List<T>());
            }
        }

        private IList<IErrorResult> _errors;
        public IList<IErrorResult> Errors
        {
            get
            {
                return _errors ?? (_errors = new List<IErrorResult>());
            }
        }

        public bool HasErrors => Errors.Any();

        IList<ILinkResult> links;
        public IList<ILinkResult> Links
        {
            get
            {
                return links ?? (links = new List<ILinkResult>());
            }
        }

        public string Token { get; set; }

        #endregion
    }
}

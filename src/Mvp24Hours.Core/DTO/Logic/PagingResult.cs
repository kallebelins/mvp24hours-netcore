//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System.Collections.Generic;

namespace Mvp24Hours.Core.DTO.Logic
{
    public class PagingResult<T> : BusinessResult<T>, IPagingResult<T>
    {
        #region [ Ctor ]

        public PagingResult()
             : base()
        {
        }

        public PagingResult(IList<T> data)
            : base(data)
        {
        }

        #endregion

        #region [ Properties ]

        public IPageResult Paging { get; set; }

        public ISummaryResult Summary { get; set; }

        #endregion
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.DTOs.Models;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mvp24Hours.Core.Extensions
{
    public static class BusinessExtensions
    {
        public static IPagingCriteria ToPagingCriteria(this PagingCriteriaRequest request)
        {
            return new PagingCriteria(
                request.Limit,
                request.Offset,
                new ReadOnlyCollection<string>(request.OrderBy ?? new List<string>()),
                new ReadOnlyCollection<string>(request.Navigation ?? new List<string>())
            );
        }
    }
}

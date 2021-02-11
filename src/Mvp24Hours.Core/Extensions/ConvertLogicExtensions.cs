using Mvp24Hours.Core.DTOs.Models;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.ObjectModel;

namespace Mvp24Hours.Core.Extensions
{
    public static class ConvertLogicExtensions
    {
        public static PagingCriteria ToPagingService(this PagingCriteriaRequest request)
        {
            return new PagingCriteria(
                request.Limit,
                request.Offset,
                new ReadOnlyCollection<string>(request.OrderBy),
                new ReadOnlyCollection<string>(request.Navigation)
            );
        }
    }
}

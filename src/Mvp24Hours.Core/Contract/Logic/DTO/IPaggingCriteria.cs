using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Logic.DTO
{
    public interface IPagingCriteria<T>
    {
        int Limit { get; set; }
        int Offset { get; set; }

        IList<string> OrderBy { get; set; }
        IList<Expression<Func<T, dynamic>>> OrderByAscendingExpr { get; set; }
        IList<Expression<Func<T, dynamic>>> OrderByDescendingExpr { get; set; }

        IList<string> Navigation { get; set; }
        IList<Expression<Func<T, dynamic>>> NavigationExpr { get; set; }
    }
}

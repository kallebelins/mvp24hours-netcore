//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);
            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }
    }
}

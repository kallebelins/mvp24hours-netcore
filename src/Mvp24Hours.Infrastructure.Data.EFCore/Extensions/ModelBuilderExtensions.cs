//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Apply filters in context
        /// </summary>
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

        public static void ApplyMvpConfigurationsFromAssembly<TEntity>(this ModelBuilder builder, Assembly assembly)
            where TEntity : class
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Configure");
                methodInfo?.Invoke(instance, new object[] { builder.Entity<TEntity>() });
            }
        }

    }
}

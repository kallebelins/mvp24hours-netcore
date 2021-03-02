//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Creates sort expression
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderExpression(source, propertyName, false);
        }
        /// <summary>
        /// Adds sort expression
        /// </summary>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderExpression(source, propertyName, true);
        }

        /// <summary>
        /// Sorting expression
        /// </summary>
        static IOrderedQueryable<T> OrderExpression<T>(this IQueryable<T> source, string propertyName, Boolean isThenBy)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");

            bool isDescending = false;
            propertyName = propertyName.Trim();

            if (propertyName.Contains('_') || propertyName.Contains(' '))
            {
                propertyName = propertyName.Replace(' ', '_');
                isDescending = propertyName.EndsWith("_desc");
                propertyName = propertyName.Replace("_asc", "").Replace("_desc", "");
            }

            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");

            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            String methodName = isDescending ? "OrderByDescending" : "OrderBy";
            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}

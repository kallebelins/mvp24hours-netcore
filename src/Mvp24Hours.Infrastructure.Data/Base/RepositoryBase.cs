//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Data
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepository"/>
    /// </summary>
    public abstract class RepositoryBase<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            this.dbEntities = dbContext.Set<T>();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Database context
        /// </summary>
        protected readonly DbContext dbContext;
        /// <summary>
        /// Represents relationship with entities in the database
        /// </summary>
        protected DbSet<T> dbEntities;
        /// <summary>
        /// Gets the value of the user logged in the context or logged into the database
        /// </summary>
        protected abstract object EntityLogBy { get; }

        #endregion

        #region [ Methods ]
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IPagingCriteria<T> clause, bool onlyNavigation = false)
        {
            // cria query
            var query = this.dbEntities.AsQueryable();
            return GetQuery(query, clause, onlyNavigation);
        }
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IQueryable<T> query, IPagingCriteria<T> clause, bool onlyNavigation = false)
        {
            var ordered = false;

            if (clause != null)
            {
                // navigation by expression
                if (clause.NavigationExpr != null && clause.NavigationExpr.Count > 0)
                {
                    foreach (var nav in clause.NavigationExpr)
                    {
                        query = query.Include(nav);
                    }
                }

                // navigation by string
                if (clause.Navigation != null && clause.Navigation.Count > 0)
                {
                    foreach (var nav in clause.Navigation)
                    {
                        query = query.Include(nav);
                    }
                }
            }


            if (!onlyNavigation)
            {
                int offset = 0;
                int limit = MaxQtyByQueryPage;

                if (clause != null)
                {
                    // ordination by ascending expression
                    if (clause.OrderByAscendingExpr != null && clause.OrderByAscendingExpr.Count > 0)
                    {
                        IOrderedQueryable<T> queryOrdered = null;
                        foreach (var ord in clause.OrderByAscendingExpr)
                        {
                            if (queryOrdered == null)
                            {
                                ordered = true;
                                queryOrdered = query.OrderBy(ord);
                            }
                            else
                            {
                                queryOrdered = queryOrdered.ThenBy(ord);
                            }
                        }
                        query = queryOrdered ?? query;
                    }

                    // ordination by descending expression
                    if (clause.OrderByDescendingExpr != null && clause.OrderByDescendingExpr.Count > 0)
                    {
                        IOrderedQueryable<T> queryOrdered = null;
                        foreach (var ord in clause.OrderByDescendingExpr)
                        {
                            if (queryOrdered == null)
                            {
                                ordered = true;
                                queryOrdered = query.OrderByDescending(ord);
                            }
                            else
                            {
                                queryOrdered = queryOrdered.ThenByDescending(ord);
                            }
                        }
                        query = queryOrdered ?? query;
                    }

                    // ordination by string
                    if (clause.OrderBy != null && clause.OrderBy.Count > 0)
                    {
                        IOrderedQueryable<T> queryOrdered = null;
                        foreach (var ord in clause.OrderBy)
                        {
                            if (queryOrdered == null)
                            {
                                ordered = true;
                                queryOrdered = query.OrderBy(ord);
                            }
                            else
                            {
                                queryOrdered = queryOrdered.ThenBy(ord);
                            }
                        }
                        query = queryOrdered ?? query;
                    }

                    // Paging
                    offset = clause.Offset;
                    limit = clause.Limit > 0 ? clause.Limit : limit;
                }

                if (!ordered)
                {
                    query = SortByKey(query, GetKeyInfo());
                }

                // page index
                query = query.Skip(limit * offset);

                // number of records per page
                query = query.Take(limit);
            }

            return query;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Maximum amount returned in query
        /// </summary>
        protected int MaxQtyByQueryPage
        {
            get
            {
                return ConfigurationPropertiesHelper.MaxQtyByQueryPage;
            }
        }

        #endregion

        #region [ Supports ]

        PropertyInfo _keyInfo;
        /// <summary>
        /// 
        /// </summary>
        protected PropertyInfo GetKeyInfo()
        {
            if (_keyInfo == null)
            {
                _keyInfo = typeof(T).GetTypeInfo()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.GetCustomAttribute<KeyAttribute>() != null)
                    .FirstOrDefault();

                if (_keyInfo == null)
                {
                    throw new InvalidOperationException("Key property not found.");
                }
            }

            return _keyInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        protected IQueryable<T> GetDynamicFilter<TValue>(IQueryable<T> query, PropertyInfo key, TValue value)
        {
            var entityParameter = Expression.Parameter(typeof(T), "e");

            var lambda =
                Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(entityParameter, key),
                        ((value != null && value.GetType() == key.PropertyType) || typeof(TValue) == key.PropertyType)
                            ? (Expression)Expression.Constant(value)
                                : (Expression)Expression.Convert(Expression.Constant(value), key.PropertyType)),
                        entityParameter);

            return query.Where(lambda);
        }
        /// <summary>
        /// 
        /// </summary>
        protected IQueryable<T> SortByKey(IQueryable<T> query, PropertyInfo key)
        {
            try
            {
                Type t = typeof(T);
                var param = Expression.Parameter(t);

                return query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        "OrderBy",
                        new Type[] { t, key.PropertyType },
                        query.Expression,
                        Expression.Quote(
                            Expression.Lambda(
                                Expression.Property(param, key),
                                param))
                    ));
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }

        #endregion
    }
}

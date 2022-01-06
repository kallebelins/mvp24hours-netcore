//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions.Data;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Data.MongoDb.Base
{
    /// <summary>
    ///  <see cref="Core.Contract.Data.IRepository"/>
    /// </summary>
    public abstract class RepositoryBase<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        protected RepositoryBase(Mvp24HoursContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            dbEntities = dbContext.Set<T>();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Database context
        /// </summary>
        protected readonly Mvp24HoursContext dbContext;
        /// <summary>
        /// Represents relationship with entities in the database
        /// </summary>
        protected IMongoCollection<T> dbEntities;
        /// <summary>
        /// Gets the value of the user logged in the context or logged into the database
        /// </summary>
        protected abstract object EntityLogBy { get; }

        #endregion

        #region [ Methods ]
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IPagingCriteria clause, bool onlyNavigation = false)
        {
            // cria query
            var query = dbEntities.AsQueryable();
            return GetQuery(query, clause, onlyNavigation);
        }
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IQueryable<T> query, IPagingCriteria clause, bool onlyNavigation = false)
        {
            var ordered = false;

            if (!onlyNavigation)
            {
                int offset = 0;
                int limit = MaxQtyByQueryPage;

                if (clause != null)
                {
                    // ordination
                    if (clause is IPagingCriteriaExpression<T>)
                    {
                        var clauseExpr = clause as IPagingCriteriaExpression<T>;
                        // ordination by ascending expression
                        if (clauseExpr.OrderByAscendingExpr.AnyOrNotNull())
                        {
                            IOrderedQueryable<T> queryOrdered = null;
                            foreach (var ord in clauseExpr.OrderByAscendingExpr)
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
                        if (clauseExpr.OrderByDescendingExpr.AnyOrNotNull())
                        {
                            IOrderedQueryable<T> queryOrdered = null;
                            foreach (var ord in clauseExpr.OrderByDescendingExpr)
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
                    }

                    // ordination by string
                    if (clause.OrderBy.AnyOrNotNull())
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

            if (clause != null)
            {
                // navigation
                if (clause is IPagingCriteriaExpression<T>)
                {
                    var clauseExpr = clause as IPagingCriteriaExpression<T>;
                    // navigation by expression
                    if (clauseExpr.NavigationExpr.AnyOrNotNull())
                    {
                        throw new NotSupportedException("Relationship loading via navigation not available for mongodb. Do data structure analysis or implement your custom repository.");
                    }
                }

                // navigation by string
                if (clause.Navigation.AnyOrNotNull())
                {
                    throw new NotSupportedException("Relationship loading via navigation not available for mongodb. Do data structure analysis or implement your custom repository.");
                }
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

        protected FilterDefinition<T> GetKeyFilter(T entity)
        {
            var key = GetKeyInfo();
            return Builders<T>.Filter.Eq(key.Name, entity.EntityKey);
        }

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
                    .FirstOrDefault(x => x.GetCustomAttribute<BsonIdAttribute>() != null);

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
                        value != null && value.GetType() == key.PropertyType || typeof(TValue) == key.PropertyType
                            ? Expression.Constant(value)
                                : Expression.Convert(Expression.Constant(value), key.PropertyType)),
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

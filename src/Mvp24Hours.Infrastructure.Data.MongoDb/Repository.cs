//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Data.MongoDb.Base;
using Mvp24Hours.Infrastructure.Data.MongoDb.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    public class Repository<T> : RepositoryBase<T>, IRepository<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public Repository(Mvp24HoursContext dbContext, IOptions<MongoDbRepositoryOptions> options)
            : base(dbContext, options)
        {
        }

        #endregion

        #region [ IQuery ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.ListAny()"/>
        /// </summary>
        public bool ListAny()
        {
            return GetQuery(null, true).Any();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.ListCount()"/>
        /// </summary>
        public int ListCount()
        {
            return GetQuery(null, true).Count();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.List()"/>
        /// </summary>
        public IList<T> List()
        {
            return List(null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.List(IPagingCriteria)"/>
        /// </summary>
        public IList<T> List(IPagingCriteria clause)
        {
            return GetQuery(clause).ToList();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetByAny(Expression{Func{T, bool}})"/>
        /// </summary>
        public bool GetByAny(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            return GetQuery(query, null, true).Any();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetByCount(Expression{Func{T, bool}})"/>
        /// </summary>
        public int GetByCount(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            return GetQuery(query, null, true).Count();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IList<T> GetBy(Expression<Func<T, bool>> clause)
        {
            return GetBy(clause, null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public IList<T> GetBy(Expression<Func<T, bool>> clause, IPagingCriteria criteria)
        {
            var query = this.dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            return GetQuery(query, criteria).ToList();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetById(int)"/>
        /// </summary>
        public T GetById(object id)
        {
            return GetById(id, null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public T GetById(object id, IPagingCriteria clause)
        {
            return GetDynamicFilter(GetQuery(clause, true), GetKeyInfo(), id).SingleOrDefault();
        }

        #endregion

        #region [ ICommand ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.Add(T)"/>
        /// </summary>
        public void Add(T entity)
        {
            if (entity == null)
            {
                return;
            }
            dbEntities.InsertOne(entity);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.Add(IList{T})"/>
        /// </summary>
        public void Add(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.Add(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Modify(T)"/>
        /// </summary>
        public void Modify(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entityDb = dbContext.Set<T>().Find(GetKeyFilter(entity)).FirstOrDefault();

            if (entityDb == null)
            {
                throw new InvalidOperationException("Key value not found.");
            }

            // properties that can not be changed

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                var entityDbLog = entityDb as IEntityLog<object>;
                entityLog.Created = entityDbLog.Created;
                entityLog.CreatedBy = entityDbLog.CreatedBy;
                entityLog.Modified = entityDbLog.Modified;
                entityLog.ModifiedBy = entityDbLog.ModifiedBy;
            }

            this.dbEntities.ReplaceOne(GetKeyFilter(entity), entity);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Modify(List)"/>
        /// </summary>
        public void Modify(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.Modify(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Remove(T)"/>
        /// </summary>
        public void Remove(T entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                entityLog.Removed = TimeZoneHelper.GetTimeZoneNow();
                entityLog.RemovedBy = EntityLogBy;
                this.Modify(entity);
            }
            else
            {
                this.ForceRemove(entity);
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Remove(List)"/>
        /// </summary>
        public void Remove(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.Remove(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Remove(int)"/>
        /// </summary>
        public void RemoveById(object id)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return;
            }

            this.Remove(entity);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Remove(IList{TEntity})"/>
        /// </summary>
        public void RemoveById(IList<object> ids)
        {
            if (ids.AnyOrNotNull())
            {
                foreach (var id in ids)
                {
                    RemoveById(id);
                }
            }
        }

        /// <summary>
        ///  If entity is not log
        /// </summary>
        private void ForceRemove(T entity)
        {
            if (entity == null)
            {
                return;
            }
            this.dbEntities.DeleteOne(GetKeyFilter(entity));
        }

        #endregion

        #region [ IQueryRelation ]
        public void LoadRelation<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            throw new NotSupportedException();
        }
        public void LoadRelation<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, bool>> clause = null, int limit = 0) where TProperty : class
        {
            throw new NotSupportedException();
        }
        public void LoadRelationSortByAscending<TProperty, TKey>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, TKey>> orderKey, Expression<Func<TProperty, bool>> clause = null, int limit = 0) where TProperty : class
        {
            throw new NotSupportedException();
        }
        public void LoadRelationSortByDescending<TProperty, TKey>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, TKey>> orderKey, Expression<Func<TProperty, bool>> clause = null, int limit = 0) where TProperty : class
        {
            throw new NotSupportedException();
        }
        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => throw new NotSupportedException();

        #endregion
    }
}

﻿//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Data.MongoDb.Base;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    public class Repository<T> : RepositoryBase<T>, IRepository<T>, IQueryRelation<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public Repository(Mvp24HoursMongoDbContext dbContext)
            : base(dbContext)
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

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQuery.Add(T)"/>
        /// </summary>
        public void Add(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.dbEntities.Add(entity);
            }
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

        #endregion

        #region [ IQueryRelation ]

        public void LoadRelation<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression)
            where TProperty : class
        {
            dbContext.Entry(entity).Reference(propertyExpression).Load();
        }

        public void LoadRelation<TProperty, TKey>(T entity,
            Expression<Func<T, IEnumerable<TProperty>>> propertyExpression,
            Expression<Func<TProperty, bool>> clause = null,
            Expression<Func<TProperty, TKey>> orderKey = null,
            Expression<Func<TProperty, TKey>> orderDescendingKey = null,
            int limit = 0)
            where TProperty : class
        {
            var query = dbContext.Entry(entity).Collection(propertyExpression).Query();

            if (clause != null)
            {
                query = query.Where(clause);
            }

            if (orderKey != null)
            {
                query = query.OrderBy(orderKey);
            }

            if (orderDescendingKey != null)
            {
                query = query.OrderByDescending(orderDescendingKey);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            query.ToList();
        }

        #endregion

        #region [ ICommand ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommand.Modify(T)"/>
        /// </summary>
        public void Modify(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entityDb = dbContext.Set<T>().Find(entity.EntityKey);

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

            dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
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
        ///  If entity is not log
        /// </summary>
        private void ForceRemove(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.dbEntities.Attach(entity);
                this.dbEntities.Remove(entity);
            }
        }

        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => null;

        #endregion
    }
}
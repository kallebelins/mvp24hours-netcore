//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryService<TEntity, TUoW> : RepositoryServiceBase<TEntity, TUoW>, IQueryService<TEntity>, ICommandService<TEntity>, IQueryRelationService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Implements IQueryService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListAny()"/>
        /// </summary>
        public virtual bool ListAny()
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().ListAny();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListCount()"/>
        /// </summary>
        public virtual int ListCount()
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().ListCount();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List()"/>
        /// </summary>
        public IList<TEntity> List()
        {
            return this.List(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IList<TEntity> List(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().List(criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetByAny(Expression{Func{T, bool}})()"/>
        /// </summary>
        public virtual bool GetByAny(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().GetByAny(clause);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetByCount(Expression{Func{T, bool}})()"/>
        /// </summary>
        public virtual int GetByCount(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().GetByCount(clause);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause)
        {
            return GetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork.GetRepository<TEntity>().GetBy(clause, criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int)"/>
        /// </summary>
        public TEntity GetById(object id)
        {
            return this.GetById(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public virtual TEntity GetById(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().GetById(id, criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion

        #region [ Implements ICommandService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Add(T)"/>
        /// </summary>
        public virtual void Add(TEntity entity)
        {
            try
            {
                if (Validate(entity))
                {
                    this.UnitOfWork.GetRepository<TEntity>().Add(entity);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Add(T)"/>
        /// </summary>
        public virtual void Add(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return;
            }

            try
            {
                foreach (var item in entities)
                {
                    Add(item);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Modify(T)"/>
        /// </summary>
        public virtual void Modify(TEntity entity)
        {
            try
            {
                if (Validate(entity))
                {
                    this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Modify(T)"/>
        /// </summary>
        public virtual void Modify(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return;
            }

            try
            {
                foreach (var item in entities)
                {
                    Modify(item);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Remove(T)"/>
        /// </summary>
        public virtual void Remove(TEntity entity)
        {
            try
            {
                this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Remove(T)"/>
        /// </summary>
        public virtual void Remove(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return;
            }

            try
            {
                foreach (var item in entities)
                {
                    Remove(item);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.RemoveById(object)"/>
        /// </summary>
        public virtual void RemoveById(object id)
        {
            try
            {
                this.UnitOfWork.GetRepository<TEntity>().RemoveById(id);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.RemoveById(T)"/>
        /// </summary>
        public virtual void RemoveById(IList<object> ids)
        {
            if (!ids.AnyOrNotNull())
            {
                return;
            }

            try
            {
                foreach (var id in ids)
                {
                    RemoveById(id);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.SaveChanges()"/>
        /// </summary>
        public virtual int SaveChanges()
        {
            try
            {
                return this.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion

        #region [ Implements IQueryRelationService ]

        public void LoadRelation<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
            where TProperty : class
        {
            try
            {
                (this.UnitOfWork.GetRepository<TEntity>() as IQueryRelation<TEntity>)?.LoadRelation(entity, propertyExpression);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        public void LoadRelation<TProperty, TKey>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            Expression<Func<TProperty, bool>> clause = null,
            Expression<Func<TProperty, TKey>> orderKey = null,
            Expression<Func<TProperty, TKey>> orderDescendingKey = null,
            int limit = 0)
            where TProperty : class
        {
            try
            {
                (this.UnitOfWork.GetRepository<TEntity>() as IQueryRelation<TEntity>)?.LoadRelation(entity, propertyExpression, clause, orderKey, orderDescendingKey, limit);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion
    }
}

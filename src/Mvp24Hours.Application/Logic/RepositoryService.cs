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
    public class RepositoryService<TEntity, TUoW> : RepositoryServiceBase<TUoW>, IQueryService<TEntity>, ICommandService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Implements IQueryService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListAny()"/>
        /// </summary>
        public virtual IBusinessResult<bool> ListAny()
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .ListAny()
                    .ToBusiness();
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
        public virtual IBusinessResult<int> ListCount()
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .ListCount()
                    .ToBusiness();
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
        public IBusinessResult<IList<TEntity>> List()
        {
            return this.List(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<IList<TEntity>> List(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .List(criteria)
                    .ToBusiness();
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
        public virtual IBusinessResult<bool> GetByAny(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByAny(clause)
                    .ToBusiness();
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
        public virtual IBusinessResult<int> GetByCount(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByCount(clause)
                    .ToBusiness();
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
        public IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause)
        {
            return GetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork
                    .GetRepository<TEntity>()
                    .GetBy(clause, criteria)
                    .ToBusiness();
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
        public IBusinessResult<TEntity> GetById(object id)
        {
            return this.GetById(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<TEntity> GetById(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetById(id, criteria)
                    .ToBusiness();
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
    }
}

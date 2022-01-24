//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryService<TEntity, TUoW> : RepositoryServiceBase<TUoW>, IQueryService<TEntity>, ICommandService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Properties ]

        private IRepository<TEntity> repository = null;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IRepository<TEntity> Repository => repository ??= UnitOfWork.GetRepository<TEntity>();

        #endregion

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
        public virtual int Add(TEntity entity)
        {
            try
            {
                if (entity.Validate())
                {
                    this.UnitOfWork.GetRepository<TEntity>().Add(entity);
                    return this.UnitOfWork.SaveChanges();
                }
                return 0;
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
        public virtual int Add(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            try
            {
                foreach (var entity in entities)
                {
                    this.UnitOfWork.GetRepository<TEntity>().Add(entity);
                }
                return this.UnitOfWork.SaveChanges();
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
        public virtual int Modify(TEntity entity)
        {
            try
            {
                if (entity.Validate())
                {
                    this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
                    return this.UnitOfWork.SaveChanges();
                }
                return 0;
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
        public virtual int Modify(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            try
            {
                foreach (var entity in entities)
                {
                    this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
                }
                return this.UnitOfWork.SaveChanges();
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
        public virtual int Remove(TEntity entity)
        {
            try
            {
                this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
                return this.UnitOfWork.SaveChanges();
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
        public virtual int Remove(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            try
            {
                foreach (var entity in entities)
                {
                    this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
                }
                return this.UnitOfWork.SaveChanges();
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
        public virtual int RemoveById(object id)
        {
            try
            {
                this.UnitOfWork.GetRepository<TEntity>().RemoveById(id);
                return this.UnitOfWork.SaveChanges();
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
        public virtual int RemoveById(IList<object> ids)
        {
            if (!ids.AnyOrNotNull())
            {
                return 0;
            }

            try
            {
                foreach (var id in ids)
                {
                    this.UnitOfWork.GetRepository<TEntity>().RemoveById(id);
                }
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

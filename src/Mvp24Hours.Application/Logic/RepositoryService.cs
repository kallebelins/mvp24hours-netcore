//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
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
    public class RepositoryService<TEntity, TUoW> : IQueryService<TEntity>, ICommandService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Properties / Fields ]

        private readonly IRepository<TEntity> repository = null;
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILoggingService logging = null;

        /// <summary>
        /// Gets unit of work instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IUnitOfWork UnitOfWork => unitOfWork;

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logging;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IRepository<TEntity> Repository => repository;

        #endregion

        #region [ Ctor ]
        /// <summary>
        /// 
        /// </summary>
        public RepositoryService(IUnitOfWork unitOfWork, ILoggingService logging)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = unitOfWork.GetRepository<TEntity>();
            this.logging = logging ?? throw new ArgumentNullException(nameof(logging));
        }
        #endregion

        #region [ Implements IQueryService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListAny()"/>
        /// </summary>
        public virtual IBusinessResult<bool> ListAny()
        {
            Logging.Trace("RepositoryService.ListAny()");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .ListAny()
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListCount()"/>
        /// </summary>
        public virtual IBusinessResult<int> ListCount()
        {
            Logging.Trace("RepositoryService.ListCount()");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .ListCount()
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List()"/>
        /// </summary>
        public IBusinessResult<IList<TEntity>> List()
        {
            Logging.Trace("RepositoryService.List()");
            return this.List(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<IList<TEntity>> List(IPagingCriteria criteria)
        {
            Logging.Trace("RepositoryService.List(IPagingCriteria)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .List(criteria)
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetByAny(Expression{Func{T, bool}})"/>
        /// </summary>
        public virtual IBusinessResult<bool> GetByAny(Expression<Func<TEntity, bool>> clause)
        {
            Logging.Trace("RepositoryService.GetByAny(Expression{Func{T, bool}})");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetByAny(clause)
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetByCount(Expression{Func{T, bool}})"/>
        /// </summary>
        public virtual IBusinessResult<int> GetByCount(Expression<Func<TEntity, bool>> clause)
        {
            Logging.Trace("RepositoryService.GetByCount(Expression{Func{T, bool}})");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetByCount(clause)
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause)
        {
            Logging.Trace("RepositoryService.GetBy(Expression{Func{T, bool}})");
            return GetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<IList<TEntity>> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            Logging.Trace("RepositoryService.GetBy(Expression{Func{T, bool}}, IPagingCriteria)");
            return UnitOfWork
                .GetRepository<TEntity>()
                .GetBy(clause, criteria)
                .ToBusiness();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int)"/>
        /// </summary>
        public IBusinessResult<TEntity> GetById(object id)
        {
            Logging.Trace("RepositoryService.GetById(int)");
            return this.GetById(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public virtual IBusinessResult<TEntity> GetById(object id, IPagingCriteria criteria)
        {
            Logging.Trace("RepositoryService.GetById(int, IPagingCriteria)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetById(id, criteria)
                .ToBusiness();
        }

        #endregion

        #region [ Implements ICommandService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Add(T)"/>
        /// </summary>
        public virtual int Add(TEntity entity)
        {
            Logging.Trace("RepositoryService.Add(T)");
            if (entity.Validate())
            {
                this.UnitOfWork.GetRepository<TEntity>().Add(entity);
                return this.UnitOfWork.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Add(IList{T})"/>
        /// </summary>
        public virtual int Add(IList<TEntity> entities)
        {
            Logging.Trace("RepositoryService.Add(IList{T})");
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            foreach (var entity in entities)
            {
                this.UnitOfWork.GetRepository<TEntity>().Add(entity);
            }
            return this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Modify(T)"/>
        /// </summary>
        public virtual int Modify(TEntity entity)
        {
            Logging.Trace("RepositoryService.Modify(T)");
            if (entity.Validate())
            {
                this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
                return this.UnitOfWork.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Modify(IList{T})"/>
        /// </summary>
        public virtual int Modify(IList<TEntity> entities)
        {
            Logging.Trace("RepositoryService.Modify(IList{T})");
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            foreach (var entity in entities)
            {
                this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
            }
            return this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Remove(T)"/>
        /// </summary>
        public virtual int Remove(TEntity entity)
        {
            Logging.Trace("RepositoryService.Remove(T)");
            this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
            return this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.Remove(IList{T})"/>
        /// </summary>
        public virtual int Remove(IList<TEntity> entities)
        {
            Logging.Trace("RepositoryService.Remove(IList{T})");
            if (!entities.AnyOrNotNull())
            {
                return 0;
            }

            foreach (var entity in entities)
            {
                this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
            }
            return this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.RemoveById(object)"/>
        /// </summary>
        public virtual int RemoveById(object id)
        {
            Logging.Trace("RepositoryService.RemoveById(object)");
            this.UnitOfWork.GetRepository<TEntity>().RemoveById(id);
            return this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.RemoveById(IList{object})"/>
        /// </summary>
        public virtual int RemoveById(IList<object> ids)
        {
            Logging.Trace("RepositoryService.RemoveById(IList{object})");
            if (!ids.AnyOrNotNull())
            {
                return 0;
            }

            foreach (var id in ids)
            {
                this.UnitOfWork.GetRepository<TEntity>().RemoveById(id);
            }
            return this.UnitOfWork.SaveChanges();
        }

        #endregion
    }
}

//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Asynchronous service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryServiceAsync<TEntity, TUoW> : IQueryServiceAsync<TEntity>, ICommandServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Properties / Fields ]

        private readonly IRepositoryAsync<TEntity> repository = null;
        private readonly IUnitOfWorkAsync unitOfWork = null;
        private readonly ILoggingService logging = null;
        private readonly IValidator<TEntity> validator = null;
        private readonly INotificationContext notificationContext = null;

        /// <summary>
        /// Gets unit of work instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IUnitOfWorkAsync UnitOfWork => unitOfWork;

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logging;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IRepositoryAsync<TEntity> Repository => repository;

        /// <summary>
        /// Defines a validator for a particular type.
        /// </summary>
        protected virtual IValidator<TEntity> Validator => validator;

        /// <summary>
        /// Context that represents a container for in-app notifications
        /// </summary>
        protected virtual INotificationContext NotificationContext => notificationContext;

        #endregion

        #region [ Ctor ]
        /// <summary>
        /// 
        /// </summary>
        public RepositoryServiceAsync(IUnitOfWorkAsync unitOfWork, ILoggingService logging)
            : this(unitOfWork, logging, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public RepositoryServiceAsync(IUnitOfWorkAsync unitOfWork, ILoggingService logging, INotificationContext notificationContext)
            : this(unitOfWork, logging, notificationContext, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActivatorUtilitiesConstructor]
        public RepositoryServiceAsync(IUnitOfWorkAsync unitOfWork, ILoggingService logging, INotificationContext notificationContext, IValidator<TEntity> validator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = unitOfWork.GetRepository<TEntity>();
            this.logging = logging ?? throw new ArgumentNullException(nameof(logging));
            this.notificationContext = notificationContext;
            this.validator = validator;
        }
        #endregion

        #region [ Implements IQueryServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAnyAsync(CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<bool>> ListAnyAsync(CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ListAnyAsync(CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .ListAnyAsync(cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListCountAsync(CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<int>> ListCountAsync(CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ListCountAsync(CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .ListCountAsync(cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(CancellationToken)"/>
        /// </summary>
        public Task<IBusinessResult<IList<TEntity>>> ListAsync(CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ListAsync(CancellationToken)");
            return this.ListAsync(null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> ListAsync(IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ListAsync(IPagingCriteria, CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .ListAsync(criteria, cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAnyAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
        /// </summary>
        public Task<IBusinessResult<bool>> GetByAnyAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByAnyAsync(Expression{Func{TEntity, bool}}, CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetByAnyAsync(clause, cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByCountAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<int>> GetByCountAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByCountAsync(Expression{Func{TEntity, bool}}, CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetByCountAsync(clause, cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
        /// </summary>
        public Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByAsync(Expression{Func{TEntity, bool}}, CancellationToken)");
            return GetByAsync(clause, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByAsync(Expression{Func{TEntity, bool}}, IPagingCriteria, CancellationToken)");
            return UnitOfWork
                .GetRepository<TEntity>()
                .GetByAsync(clause, criteria, cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(object, CancellationToken)"/>
        /// </summary>
        public Task<IBusinessResult<TEntity>> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByIdAsync(object, CancellationToken)");
            return this.GetByIdAsync(id, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(object, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public virtual Task<IBusinessResult<TEntity>> GetByIdAsync(object id, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.GetByIdAsync(object, IPagingCriteria, CancellationToken)");
            return this.UnitOfWork
                .GetRepository<TEntity>()
                .GetByIdAsync(id, criteria, cancellationToken: cancellationToken)
                .ToBusinessAsync();
        }

        #endregion

        #region [ Implements ICommandServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.AddAsync(TEntity, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.AddAsync(TEntity, CancellationToken)");
            if (entity.Validate())
            {
                await this.UnitOfWork
                    .GetRepository<TEntity>()
                    .AddAsync(entity, cancellationToken: cancellationToken);
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.AddAsync(IList{TEntity}, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> AddAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.AddAsync(IList{TEntity}, CancellationToken)");
            if (entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.AddAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.ModifyAsync(TEntity, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> ModifyAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ModifyAsync(TEntity, CancellationToken)");
            if (entity.Validate())
            {
                await this.UnitOfWork.GetRepository<TEntity>().ModifyAsync(entity, cancellationToken: cancellationToken);
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.ModifyAsync(IList{TEntity}, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> ModifyAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.ModifyAsync(IList{TEntity}, CancellationToken)");
            if (entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.ModifyAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveAsync(TEntity, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.RemoveAsync(TEntity, CancellationToken)");
            await this.UnitOfWork.GetRepository<TEntity>().RemoveAsync(entity, cancellationToken: cancellationToken);
            return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveAsync(IList{TEntity}, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> RemoveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.RemoveAsync(IList{TEntity}, CancellationToken)");
            if (entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.RemoveAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveByIdAsync(object, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.RemoveByIdAsync(object, CancellationToken)");
            await this.UnitOfWork.GetRepository<TEntity>().RemoveByIdAsync(id, cancellationToken: cancellationToken);
            return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveByIdAsync(IList{object}, CancellationToken)"/>
        /// </summary>
        public virtual async Task<int> RemoveByIdAsync(IList<object> ids, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryServiceAsync.RemoveByIdAsync(IList{object}, CancellationToken)");
            if (ids.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(ids?.Select(entity => rep.RemoveByIdAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        #endregion
    }
}

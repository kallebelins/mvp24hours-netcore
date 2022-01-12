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
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions;
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
    public class RepositoryServiceAsync<TEntity, TUoW> : RepositoryServiceBaseAsync<TUoW>, IQueryServiceAsync<TEntity>, ICommandServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Properties ]

        private IRepositoryAsync<TEntity> repository = null;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual IRepositoryAsync<TEntity> Repository => repository ??= UnitOfWork.GetRepository<TEntity>();

        #endregion

        #region [ Implements IQueryServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAnyAsync()"/>
        /// </summary>
        public virtual Task<IBusinessResult<bool>> ListAnyAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .ListAnyAsync(cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListCountAsync()"/>
        /// </summary>
        public virtual Task<IBusinessResult<int>> ListCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .ListCountAsync(cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync()"/>
        /// </summary>
        public Task<IBusinessResult<IList<TEntity>>> ListAsync(CancellationToken cancellationToken = default)
        {
            return this.ListAsync(null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> ListAsync(IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .ListAsync(criteria, cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAnyAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<IBusinessResult<bool>> GetByAnyAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByAnyAsync(clause, cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByCountAsync(Expression{Func{T, bool}})()"/>
        /// </summary>
        public virtual Task<IBusinessResult<int>> GetByCountAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByCountAsync(clause, cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            return GetByAsync(clause, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByAsync(clause, criteria, cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(int)"/>
        /// </summary>
        public Task<IBusinessResult<TEntity>> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync(id, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(int, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<TEntity>> GetByIdAsync(object id, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepository<TEntity>()
                    .GetByIdAsync(id, criteria, cancellationToken: cancellationToken)
                    .ToBusinessAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion

        #region [ Implements ICommandServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.AddAsync(TEntity)"/>
        /// </summary>
        public virtual async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await Validate(entity))
                {
                    await this.UnitOfWork
                        .GetRepository<TEntity>()
                        .AddAsync(entity, cancellationToken: cancellationToken);
                    return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
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
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.AddAsync(IList{TEntity})"/>
        /// </summary>
        public virtual async Task<int> AddAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.AddAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.ModifyAsync(TEntity)"/>
        /// </summary>
        public virtual async Task<int> ModifyAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await Validate(entity))
                {
                    await this.UnitOfWork.GetRepository<TEntity>().ModifyAsync(entity, cancellationToken: cancellationToken);
                    return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
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
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.ModifyAsync(IList{TEntity})"/>
        /// </summary>
        public virtual async Task<int> ModifyAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.ModifyAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveAsync(TEntity)"/>
        /// </summary>
        public virtual async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.UnitOfWork.GetRepository<TEntity>().RemoveAsync(entity, cancellationToken: cancellationToken);
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveAsync(IList{TEntity})"/>
        /// </summary>
        public virtual async Task<int> RemoveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (!entities.AnyOrNotNull())
            {
                var rep = this.UnitOfWork.GetRepository<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.RemoveAsync(entity, cancellationToken: cancellationToken)));
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            return 0;
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveByIdAsync(object)"/>
        /// </summary>
        public virtual async Task<int> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.UnitOfWork.GetRepository<TEntity>().RemoveByIdAsync(id, cancellationToken: cancellationToken);
                return await this.UnitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveByIdAsync(IList{object})"/>
        /// </summary>
        public virtual async Task<int> RemoveByIdAsync(IList<object> ids, CancellationToken cancellationToken = default)
        {
            if (!ids.AnyOrNotNull())
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

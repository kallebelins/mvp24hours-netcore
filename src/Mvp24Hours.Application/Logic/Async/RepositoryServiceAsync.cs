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
        #region [ Implements IQueryServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAnyAsync()"/>
        /// </summary>
        public virtual Task<IBusinessResult<bool>> ListAnyAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .ListAnyAsync(cancellationToken)
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
                    .GetRepositoryAsync<TEntity>()
                    .ListCountAsync(cancellationToken)
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
            return this.ListAsync(null, cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> ListAsync(IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .ListAsync(criteria, cancellationToken)
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
                    .GetRepositoryAsync<TEntity>()
                    .GetByAnyAsync(clause, cancellationToken)
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
                    .GetRepositoryAsync<TEntity>()
                    .GetByCountAsync(clause, cancellationToken)
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
            return GetByAsync(clause, null, cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByAsync(clause, criteria, cancellationToken)
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
            return this.GetByIdAsync(id, null, cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(int, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<TEntity>> GetByIdAsync(object id, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByIdAsync(id, criteria, cancellationToken)
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
                        .GetRepositoryAsync<TEntity>()
                        .AddAsync(entity);
                    return await this.UnitOfWork.SaveChangesAsync();
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
                var rep = this.UnitOfWork.GetRepositoryAsync<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.AddAsync(entity)));
                return await this.UnitOfWork.SaveChangesAsync();
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
                    await this.UnitOfWork.GetRepositoryAsync<TEntity>().ModifyAsync(entity);
                    return await this.UnitOfWork.SaveChangesAsync();
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
                var rep = this.UnitOfWork.GetRepositoryAsync<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.ModifyAsync(entity)));
                return await this.UnitOfWork.SaveChangesAsync();
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
                await this.UnitOfWork.GetRepositoryAsync<TEntity>().RemoveAsync(entity);
                return await this.UnitOfWork.SaveChangesAsync();
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
                var rep = this.UnitOfWork.GetRepositoryAsync<TEntity>();
                await Task.WhenAll(entities?.Select(entity => rep.RemoveAsync(entity)));
                return await this.UnitOfWork.SaveChangesAsync();
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
                await this.UnitOfWork.GetRepositoryAsync<TEntity>().RemoveByIdAsync(id);
                return await this.UnitOfWork.SaveChangesAsync();
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
                var rep = this.UnitOfWork.GetRepositoryAsync<TEntity>();
                await Task.WhenAll(ids?.Select(entity => rep.RemoveByIdAsync(entity)));
                return await this.UnitOfWork.SaveChangesAsync();
            }
            return 0;
        }

        #endregion
    }
}

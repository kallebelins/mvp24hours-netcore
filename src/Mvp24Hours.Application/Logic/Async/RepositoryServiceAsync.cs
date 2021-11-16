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

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Asynchronous service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryServiceAsync<TEntity, TUoW> : RepositoryServiceAsyncBase<TUoW>, IQueryServiceAsync<TEntity>, ICommandServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Implements IQueryServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAnyAsync()"/>
        /// </summary>
        public virtual Task<IBusinessResult<bool>> ListAnyAsync()
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .ListAnyAsync()
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
        public virtual Task<IBusinessResult<int>> ListCountAsync()
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .ListCountAsync()
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
        public Task<IBusinessResult<IList<TEntity>>> ListAsync()
        {
            return this.ListAsync(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> ListAsync(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .ListAsync(criteria)
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
        public Task<IBusinessResult<bool>> GetByAnyAsync(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByAnyAsync(clause)
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
        public virtual Task<IBusinessResult<int>> GetByCountAsync(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByCountAsync(clause)
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
        public Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<IList<TEntity>>> GetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByAsync(clause, criteria)
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
        public Task<IBusinessResult<TEntity>> GetByIdAsync(object id)
        {
            return this.GetByIdAsync(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(int, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IBusinessResult<TEntity>> GetByIdAsync(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .GetByIdAsync(id, criteria)
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
        public virtual async Task AddAsync(TEntity entity)
        {
            try
            {
                if (!(await Validate(entity)))
                {
                    await false.TaskResult();
                }
                await this.UnitOfWork
                    .GetRepositoryAsync<TEntity>()
                    .AddAsync(entity);
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
        public virtual Task AddAsync(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return Task.FromResult(false);
            }

            return Task.WhenAll(entities?.Select(x => AddAsync(x)));
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.ModifyAsync(TEntity)"/>
        /// </summary>
        public virtual async Task ModifyAsync(TEntity entity)
        {
            try
            {
                if (!(await Validate(entity)))
                {
                    await false.TaskResult();
                }
                await this.UnitOfWork.GetRepositoryAsync<TEntity>().ModifyAsync(entity);
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
        public virtual Task ModifyAsync(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return false.TaskResult();
            }

            return Task.WhenAll(entities?.Select(x => ModifyAsync(x)));
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveAsync(TEntity)"/>
        /// </summary>
        public virtual Task RemoveAsync(TEntity entity)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().RemoveAsync(entity);
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
        public virtual Task RemoveAsync(IList<TEntity> entities)
        {
            if (!entities.AnyOrNotNull())
            {
                return false.TaskResult();
            }

            return Task.WhenAll(entities?.Select(x => RemoveAsync(x)));
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.RemoveByIdAsync(object)"/>
        /// </summary>
        public virtual Task RemoveByIdAsync(object id)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().RemoveByIdAsync(id);
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
        public virtual Task RemoveByIdAsync(IList<object> ids)
        {
            if (!ids.AnyOrNotNull())
            {
                return false.TaskResult();
            }

            return Task.WhenAll(ids?.Select(x => RemoveByIdAsync(x)));
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandServiceAsync{TEntity}.SaveChangesAsync(CancellationToken)"/>
        /// </summary>
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return this.UnitOfWork.SaveChangesAsync(cancellationToken);
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

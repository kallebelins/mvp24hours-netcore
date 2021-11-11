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
    public class RepositoryServiceAsync<TEntity, TUoW> : RepositoryServiceAsyncBase<TEntity, TUoW>, IQueryServiceAsync<TEntity>, ICommandServiceAsync<TEntity>, IQueryRelationServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Implements IQueryServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAnyAsync()"/>
        /// </summary>
        public virtual Task<bool> ListAnyAsync()
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().ListAnyAsync();
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
        public virtual Task<int> ListCountAsync()
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().ListCountAsync();
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
        public Task<IList<TEntity>> ListAsync()
        {
            return this.ListAsync(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IList<TEntity>> ListAsync(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().ListAsync(criteria);
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
        public Task<bool> GetByAnyAsync(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().GetByAnyAsync(clause);
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
        public virtual Task<int> GetByCountAsync(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().GetByCountAsync(clause);
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
        public Task<IList<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<IList<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork.GetRepositoryAsync<TEntity>().GetByAsync(clause, criteria);
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
        public Task<TEntity> GetByIdAsync(object id)
        {
            return this.GetByIdAsync(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{TEntity}.GetByIdAsync(int, IPagingCriteria)"/>
        /// </summary>
        public virtual Task<TEntity> GetByIdAsync(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().GetByIdAsync(id, criteria);
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
                    await Task.FromResult(false);
                }
                await this.UnitOfWork.GetRepositoryAsync<TEntity>().AddAsync(entity);
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
                    await Task.FromResult(false);
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
                return Task.FromResult(false);
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
                return Task.FromResult(false);
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
                return Task.FromResult(false);
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

        #region [ Implements IQueryRelationServiceAsync ]

        public Task LoadRelationAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
            where TProperty : class
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().LoadRelationAsync(entity, propertyExpression);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        public Task LoadRelationAsync<TProperty, TKey>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            Expression<Func<TProperty, bool>> clause = null,
            Expression<Func<TProperty, TKey>> orderKey = null,
            Expression<Func<TProperty, TKey>> orderDescendingKey = null,
            int limit = 0)
            where TProperty : class
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<TEntity>().LoadRelationAsync(entity, propertyExpression, clause, orderKey, orderDescendingKey, limit);
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

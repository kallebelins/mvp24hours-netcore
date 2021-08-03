//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
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
        #region [ Implements IBaseAsyncBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAnyAsync()"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListCountAsync()"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync()"/>
        /// </summary>
        public Task<IList<TEntity>> ListAsync()
        {
            return this.ListAsync(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria)"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAnyAsync(Expression{Func{T, bool}})"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByCountAsync(Expression{Func{T, bool}})()"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<IList<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
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
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByIdAsync(int)"/>
        /// </summary>
        public Task<TEntity> GetByIdAsync(object id)
        {
            return this.GetByIdAsync(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByIdAsync(int, IPagingCriteria)"/>
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
                throw ex;
            }
        }

        #endregion

        #region [ Implements IManipulationBaseBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.AddAsync(T)"/>
        /// </summary>
        public virtual Task<int> AddAsync(TEntity entity)
        {
            try
            {
                bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                var validator = ServiceProviderHelper.GetService<IValidatorNotify<TEntity>>();
                if (isValidationModel && !((IValidationModel<TEntity>)entity).IsValid(validator))
                {
                    return Task.FromResult(0);
                }

                this.UnitOfWork.GetRepositoryAsync<TEntity>().AddAsync(entity);
                return this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ModifyAsync(T)"/>
        /// </summary>
        public virtual Task<int> ModifyAsync(TEntity entity)
        {
            try
            {
                bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                var validator = ServiceProviderHelper.GetService<IValidatorNotify<TEntity>>();
                if (isValidationModel && !((IValidationModel<TEntity>)entity).IsValid(validator))
                {
                    return Task.FromResult(0);
                }

                this.UnitOfWork.GetRepositoryAsync<TEntity>().ModifyAsync(entity);
                return this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.RemoveAsync(T)"/>
        /// </summary>
        public virtual Task<int> RemoveAsync(TEntity entity)
        {
            try
            {
                this.UnitOfWork.GetRepositoryAsync<TEntity>().RemoveAsync(entity);
                return this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.RemoveByIdAsync(int)"/>
        /// </summary>
        public virtual async Task<int> RemoveByIdAsync(object id)
        {
            try
            {
                var entity = await this.UnitOfWork.GetRepositoryAsync<TEntity>().GetByIdAsync(id);
                return await this.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.ICommandService{T}.SaveChangesAsync()"/>
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
                throw ex;
            }
        }

        #endregion

        #region [ Implements IQueryRelationService ]

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
                throw ex;
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
                throw ex;
            }
        }

        #endregion
    }
}

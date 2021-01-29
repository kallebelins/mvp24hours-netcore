//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base business class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryServiceAsync<T, U> : RepositoryServiceAsyncBase<T, U>, IQueryServiceAsync<T>, ICommandServiceAsync<T>
        where T : class, IEntityBase
        where U : IUnitOfWorkAsync
    {
        #region [ Implements IBaseAsyncBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAnyAsync()"/>
        /// </summary>
        public virtual Task<bool> ListAnyAsync()
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<T>().ListAnyAsync();
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
                return this.UnitOfWork.GetRepositoryAsync<T>().ListCountAsync();
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
        public Task<IList<T>> ListAsync()
        {
            return this.ListAsync(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria{T})"/>
        /// </summary>
        public virtual Task<IList<T>> ListAsync(IPagingCriteria<T> criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<T>().ListAsync(criteria);
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
        public virtual Task<int> GetByCountAsync(Expression<Func<T, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<T>().GetByCountAsync(clause);
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
        public Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria{T})"/>
        /// </summary>
        public virtual Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria<T> criteria)
        {
            try
            {
                return UnitOfWork.GetRepositoryAsync<T>().GetByAsync(clause, criteria);
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
        public Task<T> GetByIdAsync(object id)
        {
            return this.GetByIdAsync(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByIdAsync(int, IPagingCriteria{T})"/>
        /// </summary>
        public virtual Task<T> GetByIdAsync(object id, IPagingCriteria<T> criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepositoryAsync<T>().GetByIdAsync(id, criteria);
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
        public virtual Task<int> AddAsync(T entity)
        {
            try
            {
                if (typeof(T) == typeof(IValidationModel)
                    && !(entity as IValidationModel).IsValid())
                    return Task.FromResult(0);

                this.UnitOfWork.GetRepositoryAsync<T>().AddAsync(entity);
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
        public virtual Task<int> ModifyAsync(T entity)
        {
            try
            {
                if (typeof(T) == typeof(IValidationModel)
                    && !(entity as IValidationModel).IsValid())
                    return Task.FromResult(0);

                this.UnitOfWork.GetRepositoryAsync<T>().ModifyAsync(entity);
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
        public virtual Task<int> RemoveAsync(T entity)
        {
            try
            {
                this.UnitOfWork.GetRepositoryAsync<T>().RemoveAsync(entity);
                return this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.RemoveAsync(int)"/>
        /// </summary>
        public virtual Task<int> RemoveAsync(object id)
        {
            try
            {
                var entity = this.UnitOfWork.GetRepositoryAsync<T>().GetByIdAsync(id);
                return this.RemoveAsync(entity);
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
    }
}

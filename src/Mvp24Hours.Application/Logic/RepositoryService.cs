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

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryService<TEntity, TUoW> : RepositoryServiceBase<TEntity, TUoW>, IQueryService<TEntity>, ICommandService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Implements IBaseBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListAny()"/>
        /// </summary>
        public virtual bool ListAny()
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().ListAny();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListCount()"/>
        /// </summary>
        public virtual int ListCount()
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().ListCount();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List()"/>
        /// </summary>
        public IList<TEntity> List()
        {
            return this.List(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IList<TEntity> List(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().List(criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetByCount(Expression{Func{T, bool}})()"/>
        /// </summary>
        public virtual int GetByCount(Expression<Func<TEntity, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().GetByCount(clause);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause)
        {
            return GetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork.GetRepository<TEntity>().GetBy(clause, criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int)"/>
        /// </summary>
        public TEntity GetById(object id)
        {
            return this.GetById(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public virtual TEntity GetById(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<TEntity>().GetById(id, criteria);
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
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.Add(T)"/>
        /// </summary>
        public virtual int Add(TEntity entity)
        {
            try
            {
                bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                var validator = ServiceProviderHelper.GetService<IValidatorNotify<TEntity>>();
                if (isValidationModel && !((IValidationModel<TEntity>)entity).IsValid(validator))
                {
                    return 0;
                }

                this.UnitOfWork.GetRepository<TEntity>().Add(entity);
                return this.SaveChanges();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.Modify(T)"/>
        /// </summary>
        public virtual int Modify(TEntity entity)
        {
            try
            {
                bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                var validator = ServiceProviderHelper.GetService<IValidatorNotify<TEntity>>();
                if (isValidationModel && !((IValidationModel<TEntity>)entity).IsValid(validator))
                {
                    return 0;
                }

                this.UnitOfWork.GetRepository<TEntity>().Modify(entity);
                return this.SaveChanges();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.Remove(T)"/>
        /// </summary>
        public virtual int Remove(TEntity entity)
        {
            try
            {
                this.UnitOfWork.GetRepository<TEntity>().Remove(entity);
                return this.SaveChanges();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.Remove(int)"/>
        /// </summary>
        public virtual int RemoveById(object id)
        {
            try
            {
                var entity = this.UnitOfWork.GetRepository<TEntity>().GetById(id);
                return this.Remove(entity);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
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
                throw ex;
            }
        }

        #endregion
    }
}

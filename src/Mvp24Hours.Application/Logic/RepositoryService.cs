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
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="T">Represents an entity</typeparam>
    public class RepositoryService<T, U> : RepositoryServiceBase<T, U>, IQueryService<T>, ICommandService<T>
        where T : class, IEntityBase
        where U : IUnitOfWork
    {
        #region [ Implements IBaseBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.ListAny()"/>
        /// </summary>
        public virtual bool ListAny()
        {
            try
            {
                return this.UnitOfWork.GetRepository<T>().ListAny();
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
                return this.UnitOfWork.GetRepository<T>().ListCount();
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
        public IList<T> List()
        {
            return this.List(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IList<T> List(IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<T>().List(criteria);
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
        public virtual int GetByCount(Expression<Func<T, bool>> clause)
        {
            try
            {
                return this.UnitOfWork.GetRepository<T>().GetByCount(clause);
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
        public IList<T> GetBy(Expression<Func<T, bool>> clause)
        {
            return GetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IList<T> GetBy(Expression<Func<T, bool>> clause, IPagingCriteria criteria)
        {
            try
            {
                return UnitOfWork.GetRepository<T>().GetBy(clause, criteria);
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
        public T GetById(object id)
        {
            return this.GetById(id, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        public virtual T GetById(object id, IPagingCriteria criteria)
        {
            try
            {
                return this.UnitOfWork.GetRepository<T>().GetById(id, criteria);
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
        public virtual int Add(T entity)
        {
            try
            {
                if (typeof(T) == typeof(IValidationModel)
                    && !(entity as IValidationModel).IsValid())
                    return 0;

                this.UnitOfWork.GetRepository<T>().Add(entity);
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
        public virtual int Modify(T entity)
        {
            try
            {
                if (typeof(T) == typeof(IValidationModel)
                    && !(entity as IValidationModel).IsValid())
                    return 0;

                this.UnitOfWork.GetRepository<T>().Modify(entity);
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
        public virtual int Remove(T entity)
        {
            try
            {
                this.UnitOfWork.GetRepository<T>().Remove(entity);
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
                var entity = this.UnitOfWork.GetRepository<T>().GetById(id);
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

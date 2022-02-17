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
using System.Linq.Expressions;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Base service for using repository with paginated results and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryPagingService<TEntity, TUoW> : RepositoryService<TEntity, TUoW>, IQueryPagingService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : class, IUnitOfWork
    {
        #region [ Ctor ]
        /// <summary>
        /// 
        /// </summary>
        public RepositoryPagingService(TUoW _unitOfWork, ILoggingService _logging)
            : base(_unitOfWork, _logging)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public RepositoryPagingService(TUoW _unitOfWork, ILoggingService _logging, INotificationContext notificationContext)
            : base(_unitOfWork, _logging, notificationContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActivatorUtilitiesConstructor]
        public RepositoryPagingService(TUoW _unitOfWork, ILoggingService _logging, INotificationContext notificationContext, IValidator<TEntity> validator)
            : base(_unitOfWork, _logging, notificationContext, validator)
        {
        }
        #endregion

        #region [ Implements IQueryPagingService ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null)
        {
            Logging.Trace("RepositoryPagingService.GetBy(Expression{Func{T, bool}}, IPagingCriteria)");
            var repo = UnitOfWork.GetRepository<TEntity>();
            return repo.ToBusinessPaging(clause, criteria);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IPagingResult<IList<TEntity>> ListWithPagination(IPagingCriteria criteria = null)
        {
            Logging.Trace("RepositoryPagingService.List(IPagingCriteria)");
            var repo = UnitOfWork.GetRepository<TEntity>();
            return repo.ToBusinessPaging(criteria);
        }

        #endregion
    }
}

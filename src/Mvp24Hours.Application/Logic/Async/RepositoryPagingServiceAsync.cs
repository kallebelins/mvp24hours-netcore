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
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Asynchronous service for using repository with paginated results and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryPagingServiceAsync<TEntity, TUoW> : RepositoryServiceAsync<TEntity, TUoW>, IQueryPagingServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : class, IUnitOfWorkAsync
    {
        #region [ Ctor ]
        public RepositoryPagingServiceAsync(TUoW _unitOfWork, ILoggingService _logging)
            : base(_unitOfWork, _logging)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public RepositoryPagingServiceAsync(TUoW _unitOfWork, ILoggingService _logging, INotificationContext notificationContext)
            : base(_unitOfWork, _logging, notificationContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActivatorUtilitiesConstructor]
        public RepositoryPagingServiceAsync(TUoW _unitOfWork, ILoggingService _logging, INotificationContext notificationContext, IValidator<TEntity> validator)
            : base(_unitOfWork, _logging, notificationContext, validator)
        {
        }
        #endregion

        #region [ Implements IQueryPagingServiceAsync ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, CancellationToken)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryPagingServiceAsync.GetByAsync(Expression{Func{T, bool}}, CancellationToken)");
            var repo = UnitOfWork.GetRepository<TEntity>();
            return await repo.ToBusinessPagingAsync(clause, criteria);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(IPagingCriteria criteria = null, CancellationToken cancellationToken = default)
        {
            Logging.Trace("RepositoryPagingServiceAsync.ListAsync(IPagingCriteria, CancellationToken)");
            var repo = UnitOfWork.GetRepository<TEntity>();
            return await repo.ToBusinessPagingAsync(criteria);
        }

        #endregion
    }
}

//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
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
    public class RepositoryPagingService<TEntity, TUoW> : RepositoryService<TEntity, TUoW>, IQueryService<TEntity>, ICommandService<TEntity>, IQueryPagingService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Ctor ]
        /// <summary>
        /// 
        /// </summary>
        public RepositoryPagingService(IUnitOfWork _unitOfWork, ILoggingService _logging)
            : base(_unitOfWork, _logging)
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

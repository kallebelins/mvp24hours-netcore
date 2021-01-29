//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using AutoMapper;
using DriveASx.Domain.Contract.Data;
using DriveASx.Domain.Contract.Logic;
using DriveASx.Domain.DTO.Logic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveASx.Business.Logic
{
    /// <summary>
    /// Base business class mapper
    /// </summary>
    public class BaseMapperBO<T, U> : BaseBO<T, U>, IQueryMapperBO<T>, IManipulationBaseBO<T>
        where T : EntityBaseGuid
        where U : IUnitOfWork
    {
        #region [ Implements IBaseMapperBO ]

        public PaggingResult<R> MapperList<R>()
        {
            return this.MapperList<R>(null);
        }

        public Task<PaggingResult<R>> MapperListAsync<R>()
        {
            return this.MapperListAsync<R>(null);
        }

        public virtual PaggingResult<R> MapperList<R>(PaggingCriteria<T> criteria)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.ListCount();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.List(criteria as PaggingCriteria<T>);

                var result = PaggingResult<R>.Mapper(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return new PaggingResult<R>();
        }

        public virtual Task<PaggingResult<R>> MapperListAsync<R>(PaggingCriteria<T> criteria)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.ListCount();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.List(criteria);

                var result = PaggingResult<R>.TaskMapper(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return TaskResult(new PaggingResult<R>());
        }

        public PaggingResult<R> MapperGetBy<R>(Expression<Func<T, bool>> clause)
        {
            return MapperGetBy<R>(clause, null);
        }

        public Task<PaggingResult<R>> MapperGetByAsync<R>(Expression<Func<T, bool>> clause)
        {
            return MapperGetByAsync<R>(clause, null);
        }

        public virtual PaggingResult<R> MapperGetBy<R>(Expression<Func<T, bool>> clause, PaggingCriteria<T> criteria)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.GetByCount(clause);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.GetBy(clause, criteria);

                var result = PaggingResult<R>.Mapper(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return new PaggingResult<R>();
        }

        public virtual Task<PaggingResult<R>> MapperGetByAsync<R>(Expression<Func<T, bool>> clause, PaggingCriteria<T> criteria)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.GetByCount(clause);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.GetBy(clause, criteria);

                var result = PaggingResult<R>.TaskMapper(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return TaskResult(new PaggingResult<R>());
        }

        public R MapperGetById<R>(object id)
        {
            return this.MapperGetById<R>(id, null);
        }

        public Task<R> MapperGetByIdAsync<R>(object id)
        {
            return this.MapperGetByIdAsync<R>(id, null);
        }

        public virtual R MapperGetById<R>(object id, PaggingCriteria<T> criteria)
        {
            try
            {
                PaggingResult<T>.InitializeMapper<R>();
                var result = this.UnitOfWork.GetRepository<T>().GetById(id, criteria);
                return Mapper.Map<T, R>(result);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
            return default(R);
        }

        public virtual Task<R> MapperGetByIdAsync<R>(object id, PaggingCriteria<T> criteria)
        {
            try
            {
                PaggingResult<T>.InitializeMapper<R>();
                return Task.FromResult(Mapper.Map<T, R>(this.UnitOfWork.GetRepository<T>().GetById(id, criteria)));
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }

            return TaskResult(default(R));
        }

        #endregion
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryAsync"/>
    /// </summary>
    public class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public RepositoryAsync(DbContext dbContext)
            : base(dbContext)
        {
        }

        #endregion

        #region [ IQueryAsync ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAnyAsync()"/>
        /// </summary>
        public Task<bool> ListAnyAsync()
        {
            return GetQuery(null, true).AnyAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListCountAsync()"/>
        /// </summary>
        public Task<int> ListCountAsync()
        {
            return GetQuery(null, true).CountAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAsync()"/>
        /// </summary>
        public Task<IList<T>> ListAsync()
        {
            return ListAsync(null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAsync(IPagingCriteria{T})"/>
        /// </summary>
        public async Task<IList<T>> ListAsync(IPagingCriteria<T> clause)
        {
            return await GetQuery(clause).ToListAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAnyAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<bool> GetByAnyAsync(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return GetQuery(query, null, true).AnyAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByCountAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<int> GetByCountAsync(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return GetQuery(query, null, true).CountAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        public Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria{T})"/>
        /// </summary>
        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria<T> criteria)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return await GetQuery(query, criteria).ToListAsync();
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByIdAsync(int)"/>
        /// </summary>
        public Task<T> GetByIdAsync(object id)
        {
            return GetByIdAsync(id, null);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByIdAsync(int, IPagingCriteria{T})"/>
        /// </summary>
        public Task<T> GetByIdAsync(object id, IPagingCriteria<T> clause)
        {
            return GetDynamicFilter(GetQuery(clause, true), GetKeyInfo(), id).SingleOrDefaultAsync();
        }

        #endregion

        #region [ ICommandAsync ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.AddAsync(T)"/>
        /// </summary>
        public void AddAsync(T entity)
        {
            if (entity == null) return;

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.dbEntities.AddAsync(entity);
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.AddAsync(IList{T})"/>
        /// </summary>
        public void AddAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.AddAsync(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.ModifyAsync(T)"/>
        /// </summary>
        public void ModifyAsync(T entity)
        {
            if (entity == null) return;

            var entityDb = dbContext.Set<T>().Find(entity.EntityKey);

            if (entityDb == null)
            {
                throw new InvalidOperationException("Key value not found.");
            }

            // properties that can not be changed

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                var entityDbLog = entityDb as IEntityLog<object>;
                entityLog.Created = entityDbLog.Created;
                entityLog.CreatedBy = entityDbLog.CreatedBy;
                entityLog.Modified = entityDbLog.Modified;
                entityLog.ModifiedBy = entityDbLog.ModifiedBy;
            }

            dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.ModifyAsync(IList{T})"/>
        /// </summary>
        public void ModifyAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.ModifyAsync(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(T)"/>
        /// </summary>
        public void RemoveAsync(T entity)
        {
            if (entity == null) return;


            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                entityLog.Removed = TimeZoneHelper.GetTimeZoneNow();
                entityLog.RemovedBy = EntityLogBy;
                this.ModifyAsync(entity);
            }
            else
            {
                this.ForceRemoveAsync(entity);
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(IList{T})"/>
        /// </summary>
        public void RemoveAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    this.RemoveAsync(entity);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(int)"/>
        /// </summary>
        public void RemoveAsync(object id)
        {
            var entity = this.GetByIdAsync(id);
            if (entity == null) return;
            this.RemoveAsync(entity);
        }

        public void ForceRemoveAsync(T entity)
        {
            if (entity == null) return;

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.dbEntities.Attach(entity);
                this.dbEntities.Remove(entity);
            }
        }

        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => null;

        #endregion
    }
}

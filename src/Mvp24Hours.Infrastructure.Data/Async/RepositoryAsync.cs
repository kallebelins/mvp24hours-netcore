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
    ///// <summary>
    /////  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryAsync"/>
    ///// </summary>
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

        public Task<bool> ListAnyAsync()
        {
            return GetQuery(null, true).AnyAsync();
        }

        public Task<int> ListCountAsync()
        {
            return GetQuery(null, true).CountAsync();
        }

        public Task<IList<T>> ListAsync()
        {
            return ListAsync(null);
        }

        public async Task<IList<T>> ListAsync(IPagingCriteria clause)
        {
            return await GetQuery(clause).ToListAsync();
        }

        public Task<bool> GetByAnyAsync(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return GetQuery(query, null, true).AnyAsync();
        }

        public Task<int> GetByCountAsync(Expression<Func<T, bool>> clause)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return GetQuery(query, null, true).CountAsync();
        }

        public Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria)
        {
            var query = this.dbEntities.AsQueryable();
            query = query.Where(clause);
            return await GetQuery(query, criteria).ToListAsync();
        }

        public Task<T> GetByIdAsync(object id)
        {
            return GetByIdAsync(id, null);
        }

        public Task<T> GetByIdAsync(object id, IPagingCriteria clause)
        {
            return GetDynamicFilter(GetQuery(clause, true), GetKeyInfo(), id).SingleOrDefaultAsync();
        }

        #endregion

        #region [ ICommandAsync ]

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

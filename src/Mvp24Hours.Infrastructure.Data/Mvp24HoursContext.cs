//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Entities;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Mvp24HoursContext : DbContext
    {
        #region [ Ctor ]

        public Mvp24HoursContext()
            : base()
        {
        }

        public Mvp24HoursContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion

        #region [ Configs ]

        /// <summary>
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(DbContextOptionsBuilder)"/>
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        /// <summary>
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(ModelBuilder)"/>
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (CanApplyEntityLog)
            {
                builder.ApplyGlobalFilters<IEntityDateLog>(e => e.Removed == null);
            }
        }
        /// <summary>
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges"/>
        /// </summary>
        public override int SaveChanges()
        {
            this.ApplyLogRules();
            return base.SaveChanges();
        }
        /// <summary>
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(CancellationToken)"/>
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyLogRules();
            return base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(bool, CancellationToken)"/>
        /// </summary>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyLogRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        /// <summary>
        /// Apply log rules
        /// </summary>
        protected void ApplyLogRules()
        {
            if (!CanApplyEntityLog) return;

            // entity log and guid
            foreach (var entry in this.ChangeTracker
                .Entries()
                .Where(e =>
                    (e.Entity.GetType().BaseType.Name == typeof(IEntityLog<>).Name || e.Entity.GetType().BaseType.Name == typeof(EntityBaseLog<,>).Name)
                    && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)))
            {
                var e = (dynamic)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    e.Created = TimeZoneHelper.GetTimeZoneNow();
                    e.CreatedBy = (dynamic)EntityLogBy;

                    e.Modified = null;
                    e.ModifiedBy = null;

                    e.Removed = null;
                    e.RemovedBy = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (e.Removed == null)
                    {
                        e.Modified = TimeZoneHelper.GetTimeZoneNow();
                        e.ModifiedBy = (dynamic)EntityLogBy;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // no action
                }
            }
        }

        #endregion

        #region [ Props ]

        /// <summary>
        /// Indicates whether log control can be performed by the base context of Mvp24Hours.
        /// </summary>
        protected abstract bool CanApplyEntityLog { get; }
        /// <summary>
        /// Gets the value of the user logged in the context or logged into the database
        /// </summary>
        protected abstract object EntityLogBy { get; }

        #endregion
    }
}
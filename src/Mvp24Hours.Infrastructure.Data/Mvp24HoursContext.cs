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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (CanApplyEntityLog)
            {
                builder.ApplyGlobalFilters<IEntityDateLog>(e => e.Removed == null);
            }
        }

        public override int SaveChanges()
        {
            this.ApplyLogRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyLogRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyLogRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

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

        protected abstract bool CanApplyEntityLog { get; }
        protected abstract object EntityLogBy { get; }


        #endregion
    }
}
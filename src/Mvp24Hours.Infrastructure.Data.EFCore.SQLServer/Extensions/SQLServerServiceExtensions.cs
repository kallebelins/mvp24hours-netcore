//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.EFCore;
using Mvp24Hours.Infrastructure.Data.EFCore.SQLServer;
using Mvp24Hours.Infrastructure.Data.EFCore.SQLServer.Async;
using System;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class SQLServerServiceExtensions
    {
        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursSQLServerAsyncService<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null, Type repositoryAsync = null)
            where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWorkAsync>(x => new UnitOfWorkSQLServerAsync());

            if (repositoryAsync != null)
            {
                services.AddScoped(typeof(IRepositoryAsync<>), repositoryAsync);
            }
            else
            {
                services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            }

            if (dbFactory != null)
            {
                services.AddScoped<DbContext>(dbFactory);
            }
            else
            {
                services.AddScoped<DbContext, TDbContext>();
            }

            return services;
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursSQLServerService<TDbContext>(this IServiceCollection services, Func<IServiceProvider, TDbContext> dbFactory = null, Type repository = null)
               where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork>(x => new UnitOfWorkSQLServer());

            if (repository != null)
            {
                services.AddScoped(typeof(IRepository<>), repository);
            }
            else
            {
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            }

            if (dbFactory != null)
            {
                services.AddScoped<DbContext>(dbFactory);
            }
            else
            {
                services.AddScoped<DbContext, TDbContext>();
            }

            return services;
        }
    }
}

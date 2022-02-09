//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.EFCore;
using Mvp24Hours.Infrastructure.Data.EFCore.Configuration;
using System;

namespace Mvp24Hours.Extensions
{
    public static class EFCoreServiceExtensions
    {
        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursDbServiceAsync<TDbContext>(this IServiceCollection services,
            Func<IServiceProvider, TDbContext> dbFactory = null,
            Type repositoryAsync = null,
            Action<EFCoreRepositoryOptions> options = null)
            where TDbContext : DbContext
        {
            services.AddMvp24HoursLogging();

            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<EFCoreRepositoryOptions>(options => { });
            }

            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();

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
        public static IServiceCollection AddMvp24HoursDbService<TDbContext>(this IServiceCollection services,
            Func<IServiceProvider, TDbContext> dbFactory = null,
            Type repository = null,
            Action<EFCoreRepositoryOptions> options = null)
               where TDbContext : DbContext
        {
            services.AddMvp24HoursLogging();

            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<EFCoreRepositoryOptions>(options => { });
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();

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

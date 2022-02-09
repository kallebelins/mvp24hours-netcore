//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.MongoDb;
using Mvp24Hours.Infrastructure.Data.MongoDb.Configuration;
using System;

namespace Mvp24Hours.Extensions
{
    public static class MongoDbServiceExtensions
    {
        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDb(this IServiceCollection services,
            Action<MongoDbOptions> options = null,
            Action<MongoDbRepositoryOptions> repositoryOptions = null)
        {
            return services.AddMvp24HoursMongoDb<Mvp24HoursContext>(options, repositoryOptions);
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDb<DbContext>(this IServiceCollection services,
            Action<MongoDbOptions> options = null,
            Action<MongoDbRepositoryOptions> repositoryOptions = null)
            where DbContext : Mvp24HoursContext
        {
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "Options is required.");
            }

            services.Configure(options);

            if (repositoryOptions != null)
            {
                services.Configure(repositoryOptions);
            }

            // register db context
            services.AddScoped<Mvp24HoursContext, DbContext>();

            // register services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDbAsync(this IServiceCollection services,
            Action<MongoDbOptions> options = null,
            Action<MongoDbRepositoryOptions> repositoryOptions = null)
        {
            return services.AddMvp24HoursMongoDbAsync<Mvp24HoursContext>(options, repositoryOptions);
        }

        /// <summary>
        /// Add database context services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDbAsync<DbContext>(this IServiceCollection services,
            Action<MongoDbOptions> options = null,
            Action<MongoDbRepositoryOptions> repositoryOptions = null)
            where DbContext : Mvp24HoursContext
        {
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "Options is required.");
            }

            services.Configure(options);

            if (repositoryOptions != null)
            {
                services.Configure(repositoryOptions);
            }

            // register db context
            services.AddScoped<Mvp24HoursContext, DbContext>();

            // register services
            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            return services;
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.MongoDb;
using System;

namespace Mvp24Hours.Extensions
{
    public static class MongoDbServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDb<DbContext>(this IServiceCollection services, string databaseName, string connectionString)
            where DbContext : Mvp24HoursContext
        {
            services.AddMvp24HoursLogging();

            // register db context
            services.AddScoped(options =>
            {
                return (Mvp24HoursContext)Activator.CreateInstance(typeof(DbContext), databaseName, connectionString);
            });

            // register services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDb(this IServiceCollection services, string databaseName, string connectionString)
        {
            services.AddMvp24HoursLogging();

            // register db context
            services.AddScoped(options =>
            {
                return new Mvp24HoursContext(databaseName, connectionString);
            });

            // register services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDbAsync<DbContext>(this IServiceCollection services, string databaseName, string connectionString)
            where DbContext : Mvp24HoursContext
        {
            services.AddMvp24HoursLogging();

            // register db context
            services.AddScoped(options =>
            {
                return (Mvp24HoursContext)Activator.CreateInstance(typeof(DbContext), databaseName, connectionString);
            });

            // register services
            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDbAsync(this IServiceCollection services, string databaseName, string connectionString)
        {
            services.AddMvp24HoursLogging();

            // register db context
            services.AddScoped(options =>
            {
                return new Mvp24HoursContext(databaseName, connectionString);
            });

            // register services
            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            return services;
        }
    }
}

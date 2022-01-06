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

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class MongoDbServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddMvp24HoursMongoDb(this IServiceCollection services, string databaseName, string connectionString)
        {
            // register db context
            services.AddScoped(options =>
            {
                return new Mvp24HoursContext(databaseName, connectionString);
            });

            // register services
            services.AddScoped<IUnitOfWork>(x => new UnitOfWork());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}

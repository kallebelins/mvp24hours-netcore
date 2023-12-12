using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Data.EFCore;
using System;
using System.Data.SqlClient;

namespace Rottur.Shared.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,
                                            Action<TContext, IServiceProvider> seeder,
                                            int? retry = 0) where TContext : Mvp24HoursContext
        {
            TelemetryHelper.Execute(TelemetryLevel.Information, "infa-database-migrate-start", $"context:{typeof(TContext).Name}");
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<TContext>();

                try
                {
                    TelemetryHelper.Execute(TelemetryLevel.Information, "infa-database-migrate-invokeseeder");
                    InvokeSeeder(seeder, context, services);
                }
                catch (SqlException ex)
                {
                    TelemetryHelper.Execute(TelemetryLevel.Error, "infa-database-migrate-failure", ex);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        host.MigrateDatabase(seeder, retryForAvailability);
                    }
                }
                finally
                {
                    TelemetryHelper.Execute(TelemetryLevel.Information, "infa-database-migrate-end", $"context:{typeof(TContext).Name}");
                }
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                                    TContext context,
                                                    IServiceProvider services)
                                                    where TContext : Mvp24HoursContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Runs query on database
        /// </summary>
        public static Task<IList<T>> ExecuteQueryAsync<T>(this DbContext context, string sqlQuery, params object[] parameters) where T : class
        {
            return context.SqlQueryAsync<T>(sqlQuery, parameters);
        }

        /// <summary>
        /// Runs commands on database
        /// </summary>
        public static Task<int> ExecuteCommandAsync(this DbContext context, string sqlCommand, params object[] parameters)
        {
            return context.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

        /// <summary>
        /// Runs commands on database
        /// </summary>
        public static async Task<int> ExecuteNonQueryAsync(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using var command = conn.CreateCommand();
            command.CommandText = rawSql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }

            await conn.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Execute scalar command against database
        /// </summary>
        public static async Task<T> ExecuteScalarAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using var command = conn.CreateCommand();
            command.CommandText = rawSql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }

            await conn.OpenAsync();
            return (T)await command.ExecuteScalarAsync();
        }

        /// <summary>
        /// Run query commands in context
        /// </summary>
        internal static IList<T> SqlQuery<T>(this DbContext db, string sql, params object[] parameters)
            where T : class
        {
            return db.Set<T>().FromSqlRaw(sql, parameters).ToList();
        }

        /// <summary>
        /// Executes asynchronous query commands in context
        /// </summary>
        internal static async Task<IList<T>> SqlQueryAsync<T>(this DbContext db, string sql, params object[] parameters)
            where T : class
        {
            return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }
    }
}

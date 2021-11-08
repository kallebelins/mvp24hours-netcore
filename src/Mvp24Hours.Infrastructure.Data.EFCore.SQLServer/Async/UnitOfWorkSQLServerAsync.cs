//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.EFCore.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore.SQLServer.Async
{
    public class UnitOfWorkSQLServerAsync : UnitOfWorkAsync, ISQLAsync
    {
        public UnitOfWorkSQLServerAsync()
            : base()
        {
        }

        public Task<IList<T>> ExecuteQueryAsync<T>(string sqlQuery, params object[] parameters) where T : class
        {
            return this.DbContext.SqlQueryAsync<T>(sqlQuery, parameters);
        }

        public Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            return this.DbContext.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

    }
}

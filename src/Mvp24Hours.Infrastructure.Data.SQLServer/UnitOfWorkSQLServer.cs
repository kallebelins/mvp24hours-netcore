//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.Extensions;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Data.SQLServer
{
    public class UnitOfWorkSQLServer : Data.UnitOfWork, ISQL
    {
        public UnitOfWorkSQLServer()
            : base()
        {
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ISQL.ExecuteQuery(string, object[])"/>
        /// </summary>
        public IList<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class
        {
            return this.DbContext.SqlQuery<T>(sqlQuery, parameters);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ISQL.ExecuteCommand(string, object[])"/>
        /// </summary>
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this.DbContext.Database.ExecuteSqlRaw(sqlCommand, parameters);
        }

    }
}

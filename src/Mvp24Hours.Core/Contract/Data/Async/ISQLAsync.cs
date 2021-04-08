//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Data
{
    /// <summary>
    /// Contract for projection / query commands and data manipulation.
    /// </summary>
    public interface ISQLAsync
    {
        /// <summary>
        /// Execute command for data projection - query - in the database.
        /// </summary>
        /// <typeparam name="T">Entity type that contains properties that match the projected columns.</typeparam>
        /// <param name="sqlQuery">
        /// Dialect Query
        /// <example>
        /// SELECT ID, Name FROM dbo.[Client] WHERE ID = {0}
        /// </example>
        /// </param>
        /// <param name="parameters">Parameter vector</param>
        /// <returns>
        /// List with items projected in the query
        /// </returns>
        Task<IList<T>> ExecuteQueryAsync<T>(string sqlQuery, params object[] parameters) where T : class;

        /// <summary>
        /// Execute arbitrary commands in the database.
        /// </summary>
        /// <param name="sqlCommand">
        /// Command to execute
        /// <example>
        /// UPDATE dbo.[Customers] SET Name = \"New Name\" WHERE ID = {0}
        /// </example>
        ///</param>
        /// <param name="parameters">Parameter vector</param>
        /// <returns>Number of records affected</returns>
        Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters);
    }
}

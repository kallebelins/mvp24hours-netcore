using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;

namespace Mvp24Hours.Infrastructure.Data.EFCore.SQLServer.Async
{
    public class RepositorySQLServerAsync<T> : RepositoryAsync<T>, IRepositoryAsync<T>
            where T : class, IEntityBase
    {
        #region [ Ctor ]

        public RepositorySQLServerAsync(DbContext dbContext)
            : base(dbContext)
        {
        }

        #endregion

        #region [ Overrides ]

        #endregion
    }
}

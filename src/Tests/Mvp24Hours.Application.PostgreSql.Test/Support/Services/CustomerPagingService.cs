//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Logic;
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;

namespace Mvp24Hours.Application.PostgreSql.Test.Support.Services
{
    public class CustomerPagingService : RepositoryPagingService<Customer, IUnitOfWork>
    {
        public CustomerPagingService(IUnitOfWork unitOfWork, ILoggingService logging)
            : base(unitOfWork, logging) { }

        // custom methods
    }
}

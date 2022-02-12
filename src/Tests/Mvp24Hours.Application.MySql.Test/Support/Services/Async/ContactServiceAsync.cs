//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Logic;
using Mvp24Hours.Application.MySql.Test.Support.Entities;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;

namespace Mvp24Hours.Application.MySql.Test.Support.Services.Async
{
    public class ContactServiceAsync : RepositoryServiceAsync<Customer, IUnitOfWorkAsync>
    {
        public ContactServiceAsync(IUnitOfWorkAsync unitOfWork, ILoggingService logging)
            : base(unitOfWork, logging) { }

        // custom methods here
    }
}

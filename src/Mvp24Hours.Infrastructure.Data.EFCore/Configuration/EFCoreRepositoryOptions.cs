using Mvp24Hours.Core.Helpers;
using System.Transactions;

namespace Mvp24Hours.Infrastructure.Data.EFCore.Configuration
{
    public sealed class EFCoreRepositoryOptions
    {
        public int MaxQtyByQueryPage { get; set; } = ContantsHelper.Data.MaxQtyByQueryPage;
        public IsolationLevel? TransactionIsolationLevel { get; set; }
    }
}

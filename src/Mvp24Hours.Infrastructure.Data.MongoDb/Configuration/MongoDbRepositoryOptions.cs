using Mvp24Hours.Core.Helpers;

namespace Mvp24Hours.Infrastructure.Data.MongoDb.Configuration
{
    public sealed class MongoDbRepositoryOptions
    {
        public int MaxQtyByQueryPage { get; set; } = ContantsHelper.Data.MaxQtyByQueryPage;
    }
}

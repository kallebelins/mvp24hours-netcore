using Microsoft.Extensions.Caching.Distributed;

namespace Mvp24Hours.Infrastructure.Data.Redis.Base
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryCacheAsync{T}"/>
    /// </summary>
    public class RepositoryCacheBase
    {
        private readonly IDistributedCache _cache;

        public RepositoryCacheBase(IDistributedCache cache)
        {
            _cache = cache ?? throw new System.ArgumentNullException("An instance of IDistributedCache is required for this key and value repository.");
        }

        protected virtual IDistributedCache Cache => _cache;
    }

}

//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.Redis.Base;
using Mvp24Hours.Infrastructure.Extensions;

namespace Mvp24Hours.Infrastructure.Data.Cache
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryCache{T}"/>
    /// </summary>
    public class RepositoryCache<T> : RepositoryCacheBase, IRepositoryCache<T>
    {
        public RepositoryCache(IDistributedCache cache)
            : base(cache)
        {
        }

        public virtual T Get(string key)
        {
            return Cache.GetCacheObject<T>(key);
        }

        public virtual string GetString(string key)
        {
            return Cache.GetCacheString(key);
        }

        public virtual void Set(string key, T model)
        {
            Cache.SetCacheObject(key, model);
        }

        public virtual void SetString(string key, string value)
        {
            Cache.SetCacheString(key, value);
        }

        public virtual void Remove(string key)
        {
            Cache.RemoveCacheString(key);
        }
    }
}

//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Caching.Base;

namespace Mvp24Hours.Infrastructure.Caching
{
    /// <summary>
    ///  <see cref="IRepositoryCache{T}"/>
    /// </summary>
    public class RepositoryCache<T> : RepositoryCacheBase, IRepositoryCache<T>
        where T : class
    {
        public RepositoryCache(IDistributedCache cache)
            : base(cache)
        {
        }

        public virtual T Get(string key)
        {
            return Cache.GetObject<T>(key);
        }

        public virtual string GetString(string key)
        {
            return Cache.GetString(key);
        }

        public virtual void Set(string key, T model)
        {
            Cache.SetObject(key, model);
        }

        public virtual void SetString(string key, string value)
        {
            Cache.SetString(key, value);
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}

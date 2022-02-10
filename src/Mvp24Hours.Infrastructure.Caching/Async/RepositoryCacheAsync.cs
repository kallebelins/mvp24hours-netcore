//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Caching.Base;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Caching
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryCacheAsync{T}"/>
    /// </summary>
    public class RepositoryCacheAsync<T> : RepositoryCacheBase, IRepositoryCacheAsync<T>
        where T : class
    {
        public RepositoryCacheAsync(IDistributedCache cache)
            : base(cache)
        {
        }

        public virtual async Task<T> GetAsync(string key)
        {
            return await Cache.GetObjectAsync<T>(key);
        }

        public virtual async Task<string> GetStringAsync(string key)
        {
            return await Cache.GetStringAsync(key);
        }

        public virtual async Task SetAsync(string key, T model)
        {
            await Cache.SetObjectAsync(key, model);
        }

        public virtual async Task SetStringAsync(string key, string value)
        {
            await Cache.SetStringAsync(key, value);
        }

        public virtual async Task RemoveAsync(string key)
        {
            await Cache.RemoveAsync(key);
        }
    }
}

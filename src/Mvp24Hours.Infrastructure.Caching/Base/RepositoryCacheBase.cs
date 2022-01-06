//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;

namespace Mvp24Hours.Infrastructure.Caching.Base
{
    /// <summary>
    ///  <see cref="Core.Contract.Data.IRepositoryCacheAsync{T}"/>
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

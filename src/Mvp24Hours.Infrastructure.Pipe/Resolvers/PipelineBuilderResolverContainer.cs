using Mvp24Hours.Extensions;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Pipe.Resolvers
{
    /// <summary>
    /// 
    /// </summary>
    public class PipelineBuilderResolverContainer<TService>
    {
        private readonly Dictionary<string, PipelineBuilderResolver> _resolvers;
        private readonly string _keyDefault;

        /// <summary>
        /// 
        /// </summary>
        public PipelineBuilderResolverContainer()
        : this(null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public PipelineBuilderResolverContainer(string keyDefault)
        {
            _resolvers = new Dictionary<string, PipelineBuilderResolver>();

            if (keyDefault.HasValue())
            {
                _keyDefault = keyDefault;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PipelineBuilderResolverContainer<TService> Add(string key, PipelineBuilderResolver obj)
        {
            if (_resolvers.ContainsKey(key))
            {
                _resolvers[key] = obj;
            }
            else
            {
                _resolvers.Add(key, obj);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public PipelineBuilderResolver GetDefault()
        {
            if (_keyDefault.HasValue())
            {
                return Get(_keyDefault);
            }
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        public PipelineBuilderResolver Get(string key)
        {
            if (_resolvers.ContainsKey(key))
            {
                return _resolvers[key];
            }
            return default;
        }
    }
}

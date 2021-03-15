using Mvp24Hours.Core.Extensions;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Pipe.Resolvers
{
    public class PipelineBuilderResolverContainer<TService>
    {
        private readonly Dictionary<string, PipelineBuilderResolver> _resolvers;
        private readonly string _keyDefault;

        public PipelineBuilderResolverContainer()
        : this(null)
        {
        }

        public PipelineBuilderResolverContainer(string keyDefault)
        {
            _resolvers = new Dictionary<string, PipelineBuilderResolver>();

            if (keyDefault.HasValue())
            {
                _keyDefault = keyDefault;
            }
        }

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

        public PipelineBuilderResolver GetDefault()
        {
            if (_keyDefault.HasValue())
            {
                return Get(_keyDefault);
            }
            return default;
        }

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

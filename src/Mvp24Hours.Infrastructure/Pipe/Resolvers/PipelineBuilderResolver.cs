using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.Pipe.Resolvers
{
    public class PipelineBuilderResolver
    {
        private readonly Dictionary<string, Type> _builders;
        private readonly Dictionary<string, List<Type>> _buildersComplex;

        public PipelineBuilderResolver()
        {
            _builders = new Dictionary<string, Type>();
            _buildersComplex = new Dictionary<string, List<Type>>();
        }

        public PipelineBuilderResolver AddList<T, U>()
            where T : IPipelineBuilder
            where U : IPipelineBuilder, new()
        {
            return AddList<T, U>(typeof(T).FullName);
        }

        public PipelineBuilderResolver AddList<T, U>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
            where U : IPipelineBuilder, new()
        {
            string keyName = key;
            if (!isSimpleKey)
            {
                keyName = $"{key}_{typeof(T).FullName}";
            }

            if (!_buildersComplex.ContainsKey(keyName))
            {
                _buildersComplex.Add(keyName, new List<Type>());
            }

            _buildersComplex[keyName].Add(typeof(U));
            return this;
        }

        public PipelineBuilderResolver Add<T, U>()
            where T : IPipelineBuilder
            where U : IPipelineBuilder, new()
        {
            Add<T, U>(typeof(T).FullName);
            return this;
        }

        public PipelineBuilderResolver Add<T, U>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
            where U : IPipelineBuilder, new()
        {
            string keyName = key;
            if (!isSimpleKey)
            {
                keyName = $"{key}_{typeof(T).FullName}";
            }

            if (_builders.ContainsKey(keyName))
            {
                _builders[keyName] = typeof(U);
            }
            else
            {
                _builders.Add(keyName, typeof(U));
            }

            return this;
        }

        public T Get<T>()
            where T : IPipelineBuilder
        {
            return Get<T>(typeof(T).FullName);
        }

        public T Get<T>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
        {
            string keyName = key;
            if (!isSimpleKey)
            {
                keyName = $"{key}_{typeof(T).FullName}";
            }

            if (_builders.ContainsKey(keyName))
            {
                return (T)Activator.CreateInstance(_builders[keyName]);
            }
            return default;
        }

        public List<T> GetList<T>()
            where T : IPipelineBuilder
        {
            return GetList<T>(typeof(T).FullName);
        }

        public List<T> GetList<T>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
        {
            string keyName = key;
            if (!isSimpleKey)
            {
                keyName = $"{key}_{typeof(T).FullName}";
            }

            if (_buildersComplex.ContainsKey(keyName))
            {
                var result = new List<T>();
                foreach (var item in _buildersComplex[keyName])
                {
                    result.Add((T)Activator.CreateInstance(item));
                }

                return result;
            }
            return default;
        }

        public bool Has<T>()
            where T : IPipelineBuilder
        {
            return Has(typeof(T).FullName);
        }

        public bool Has<T>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
        {
            return Has(key, isSimpleKey ? null : typeof(T));
        }

        public bool Has(string key, Type typeComposeKey = null)
        {
            string keyName = key;
            if (typeComposeKey != null)
            {
                keyName = $"{key}_{typeComposeKey.FullName}";
            }

            return _builders.ContainsKey(keyName);
        }

        public bool HasList<T>(string key, bool isSimpleKey = false)
            where T : IPipelineBuilder
        {
            return HasList(key, isSimpleKey ? null : typeof(T));
        }

        public bool HasList(string key, Type typeComposeKey = null)
        {
            string keyName = key;
            if (typeComposeKey != null)
            {
                keyName = $"{key}_{typeComposeKey.FullName}";
            }

            return _buildersComplex.ContainsKey(keyName);
        }
    }
}

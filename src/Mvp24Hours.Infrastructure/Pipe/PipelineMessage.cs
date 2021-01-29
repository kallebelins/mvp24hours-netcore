//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Pipe
{
    public class PipelineMessage : IPipelineMessage
    {
        public PipelineMessage(params object[] args)
        {
            this._contents = new Dictionary<Type, object>();
            this.IsSucess = true;

            if (args?.Count() > 0)
            {
                foreach (var item in args)
                    this._contents.Add(item.GetType(), item);
            }
        }

        private Dictionary<Type, object> _contents;
        private IList<string> _warnings;
        private bool _isLocked;

        public bool IsSucess { get; set; }

        public IList<string> Errors
        {
            get
            {
                return _warnings ?? (_warnings = new List<string>());
            }
        }

        public string Token { get; set; }

        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
        }

        public void AddContent<T>(T obj)
        {
            if (_contents.ContainsKey(typeof(T)))
            {
                _contents[typeof(T)] = obj;
            }
            else
            {
                _contents.Add(typeof(T), obj);
            }
        }

        public T GetContent<T>()
        {
            if (_contents.ContainsKey(typeof(T)))
            {
                return (T)_contents[typeof(T)];
            }
            return default;
        }

        public IList<object> GetContentAll()
        {
            return _contents.Values.ToList();
        }

        public void Lock()
        {
            _isLocked = true;
        }
    }
}

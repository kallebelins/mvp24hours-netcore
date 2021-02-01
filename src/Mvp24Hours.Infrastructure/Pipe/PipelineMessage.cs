//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipelineMessage"/>
    /// </summary>
    public class PipelineMessage : IPipelineMessage
    {
        #region [ Ctor ]

        public PipelineMessage(params object[] args)
        {
            this._contents = new Dictionary<string, object>();
            this.IsSuccess = true;

            if (args?.Count() > 0)
            {
                foreach (var item in args)
                    AddContent(item.GetType().Name, item);
            }
        }

        #endregion

        #region [ Members ]

        private Dictionary<string, object> _contents;
        private IList<IMessageResult> _messages;

        public bool IsSuccess { get; set; }

        public IList<IMessageResult> Messages
        {
            get
            {
                return _messages ?? (_messages = new List<IMessageResult>());
            }
        }

        public string Token { get; set; }

        public bool IsLocked { get; private set; }

        #endregion

        #region [ Methods ]

        public void AddContent<T>(T obj)
        {
            AddContent<T>(typeof(T).Name, obj);
        }

        public void AddContent<T>(string key, T obj)
        {
            if (_contents.ContainsKey(key))
            {
                _contents[key] = obj;
            }
            else
            {
                _contents.Add(key, obj);
            }
        }

        public T GetContent<T>()
        {
            return GetContent<T>(typeof(T).Name);
        }

        public T GetContent<T>(string key)
        {
            if (_contents.ContainsKey(key))
            {
                return (T)_contents[key];
            }
            return default;
        }

        public IList<object> GetContentAll()
        {
            return _contents.Values.ToList();
        }

        public void Lock()
        {
            IsLocked = true;
        }

        #endregion
    }
}

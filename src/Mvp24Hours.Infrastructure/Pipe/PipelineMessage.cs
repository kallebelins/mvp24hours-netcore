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
            this._contents = new Dictionary<Type, object>();
            this.IsSuccess = true;

            if (args?.Count() > 0)
            {
                foreach (var item in args)
                    this._contents.Add(item.GetType(), item);
            }
        }

        #endregion

        #region [ Members ]

        private Dictionary<Type, object> _contents;
        private IList<IMessageResult> _messages;
        private bool _isLocked;

        public bool IsSuccess { get; set; }

        public IList<IMessageResult> Messages
        {
            get
            {
                return _messages ?? (_messages = new List<IMessageResult>());
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

        #endregion

        #region [ Methods ]

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

        #endregion
    }
}

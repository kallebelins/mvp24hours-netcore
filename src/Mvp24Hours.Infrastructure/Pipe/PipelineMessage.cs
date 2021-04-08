//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
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

        public PipelineMessage()
            : this(null)
        {
        }

        public PipelineMessage(params object[] args)
        {
            this._contents = new Dictionary<string, object>();

            if (args?.Count() > 0)
            {
                foreach (var item in args)
                {
                    AddContent(item);
                }
            }
        }

        #endregion

        #region [ Fields ]
        private Dictionary<string, object> _contents;
        private IList<IMessageResult> _messages;
        private bool isSuccess = true;
        #endregion

        #region [ Properties ]

        public bool IsSuccess
        {
            get
            {
                return !Messages?.Any(x => x.Type == Core.Enums.MessageType.Error) ?? isSuccess;
            }
        }

        public IList<IMessageResult> Messages
        {
            get
            {
                return _messages ??= new List<IMessageResult>();
            }
        }

        public string Token { get; private set; }

        public bool IsLocked { get; private set; }

        #endregion

        #region [ Methods ]

        public void AddContent<T>(T obj)
        {
            if (obj == null) return;
            AddContent<T>(obj.GetType().FullName, obj);
        }

        public void AddContent<T>(string key, T obj)
        {
            if (obj == null) return;
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
            return GetContent<T>(typeof(T).FullName);
        }

        public T GetContent<T>(string key)
        {
            if (_contents.ContainsKey(key))
            {
                return (T)_contents[key];
            }
            return default;
        }

        public bool HasContent<T>()
        {
            return HasContent(typeof(T).FullName);
        }

        public bool HasContent(string key)
        {
            return _contents.ContainsKey(key);
        }

        public IList<object> GetContentAll()
        {
            return _contents.Values.ToList();
        }

        public void SetLock()
        {
            IsLocked = true;
        }

        public void SetToken(string token)
        {
            if (string.IsNullOrEmpty(Token)
                && !string.IsNullOrEmpty(token))
                Token = token;
        }

        public void SetFailure()
        {
            isSuccess = false;
        }

        #endregion
    }
}

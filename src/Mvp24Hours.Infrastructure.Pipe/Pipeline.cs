//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Pipe.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipeline"/>
    /// </summary>
    public class Pipeline : PipelineBase, IPipeline
    {
        #region [ Ctor ]
        public Pipeline()
            : base()
        {
        }
        public Pipeline(string token)
            : base(token)
        {
        }
        public Pipeline(bool isBreakOnFail)
            : base(isBreakOnFail)
        {
        }
        public Pipeline(string token, bool isBreakOnFail)
            : base(token, isBreakOnFail)
        {
        }
        #endregion

        #region [ Fields / Properties ]
        private readonly IList<IOperation> operations = new List<IOperation>();

        private readonly IDictionary<PipelineInterceptorType, IList<IOperation>> dictionaryInterceptors = new Dictionary<PipelineInterceptorType, IList<IOperation>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> preCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> postCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>>();

        private readonly IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> dictionaryEventInterceptors = new Dictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> preEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> postEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        #endregion

        #region [ Methods ]

        public IPipeline Add<T>() where T : IOperation
        {
            IOperation instance = ServiceProviderHelper.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return Add(Activator.CreateInstance<T>());
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return Add(instance);
        }
        public IPipeline Add(IOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            this.operations.Add(operation);
            return this;
        }
        public IPipeline Add(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            this.operations.Add(new OperationAction(action, isRequired));
            return this;
        }

        public IPipeline AddInterceptors<T>(PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation) where T : IOperation
        {
            IOperation instance = ServiceProviderHelper.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddInterceptors(Activator.CreateInstance<T>(), pipelineInterceptor);
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, pipelineInterceptor);
        }
        public IPipeline AddInterceptors(IOperation operation, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperation>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(operation);
            return this;
        }
        public IPipeline AddInterceptors(Action<IPipelineMessage> action, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperation>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(new OperationAction(action));
            return this;
        }
        public IPipeline AddInterceptors<T>(Func<IPipelineMessage, bool> condition, bool postOperation = true) where T : IOperation
        {
            IOperation instance = ServiceProviderHelper.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddInterceptors(Activator.CreateInstance<T>(), condition, postOperation);
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, condition, postOperation);
        }
        public IPipeline AddInterceptors(IOperation operation, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (postOperation)
            {
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, operation));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, operation));
            }
            return this;
        }
        public IPipeline AddInterceptors(Action<IPipelineMessage> action, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Condition is mandatory.");
            }
            if (postOperation)
            {
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, new OperationAction(action)));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, new OperationAction(action)));
            }
            return this;
        }
        public IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler has not been defined or is null.");
            }

            if (!this.dictionaryEventInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryEventInterceptors.Add(pipelineInterceptor, new List<EventHandler<IPipelineMessage, EventArgs>>());
            }
            this.dictionaryEventInterceptors[pipelineInterceptor].Add(handler);
            return this;
        }
        public IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler is mandatory.");
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Condition is mandatory.");
            }
            if (postOperation)
            {
                this.postEventCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>(condition, handler));
            }
            else
            {
                this.preEventCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>(condition, handler));
            }
            return this;
        }

        public IPipeline Execute(IPipelineMessage input = null)
        {
            Message = input ?? Message;
            Message = RunOperations(this.operations, Message);
            return this;
        }
        protected virtual IPipelineMessage RunOperations(IList<IOperation> _operations, IPipelineMessage input, bool onlyOperationDefault = false)
        {
            if (!_operations.AnyOrNotNull())
            {
                return input;
            }

            if (input == null)
            {
                input = new PipelineMessage();
            }

            if (!Token.HasValue())
            {
                Token = input.Token.HasValue() ? input.Token : Guid.NewGuid().ToString();
            }

            if (!onlyOperationDefault)
            {
                RunEventInterceptors(input, PipelineInterceptorType.FirstOperation);
                RunOperationInterceptors(input, PipelineInterceptorType.FirstOperation);
            }

            _ = _operations.Aggregate(input, (current, operation) =>
              {
                  current.SetToken(this.Token);

                  if ((current.IsFaulty || !IsValidContext) && this.IsBreakOnFail)
                  {
                      return current;
                  }

                  if (current.IsLocked && !operation.IsRequired)
                  {
                      return current;
                  }

                  try
                  {
                      // pre-operation
                      if (!onlyOperationDefault)
                      {
                          // events
                          RunCustomEventInterceptors(input, false);
                          RunEventInterceptors(input, PipelineInterceptorType.PreOperation);

                          // operations
                          RunCustomOperationInterceptors(input, false);
                          RunOperationInterceptors(input, PipelineInterceptorType.PreOperation);
                      }

                      // operation
                      operation.Execute(current);

                      // post-operation
                      if (!onlyOperationDefault)
                      {
                          // events
                          RunEventInterceptors(input, PipelineInterceptorType.PostOperation);
                          RunCustomEventInterceptors(input);

                          // operations
                          RunOperationInterceptors(current, PipelineInterceptorType.PostOperation);
                          RunCustomOperationInterceptors(current);

                          if (current.IsLocked)
                          {
                              RunEventInterceptors(input, PipelineInterceptorType.Locked, true);
                              RunOperationInterceptors(current, PipelineInterceptorType.Locked, true);
                          }

                          if (current.IsFaulty || !IsValidContext)
                          {
                              RunEventInterceptors(input, PipelineInterceptorType.Faulty, true);
                              RunOperationInterceptors(current, PipelineInterceptorType.Faulty, true);
                          }
                      }

                      return current;
                  }
                  catch (Exception ex)
                  {
                      current.Messages.Add(new MessageResult((ex?.InnerException ?? ex).Message, MessageType.Error));
                      input.AddContent(ex);
                  }
                  return current;
              });

            if (!onlyOperationDefault && (!input.IsFaulty || IsValidContext))
            {
                RunEventInterceptors(input, PipelineInterceptorType.LastOperation);
                RunOperationInterceptors(input, PipelineInterceptorType.LastOperation);
            }

            return input;
        }

        protected virtual void RunOperationInterceptors(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryInterceptors.ContainsKey(interceptorType))
            {
                RunOperations(dictionaryInterceptors[interceptorType], input, true);
                if (canClearList)
                {
                    dictionaryInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual void RunCustomOperationInterceptors(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in postCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            RunOperations(new List<IOperation> { ci.Value }, input, true);
                        }
                    }
                }
            }
            else
            {
                if (preCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in preCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            RunOperations(new List<IOperation> { ci.Value }, input, true);
                        }
                    }
                }
            }
        }
        protected virtual void RunEventInterceptors(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryEventInterceptors.ContainsKey(interceptorType))
            {
                RunEvents(dictionaryEventInterceptors[interceptorType], input);
                if (canClearList)
                {
                    dictionaryEventInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual void RunCustomEventInterceptors(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postEventCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in postEventCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            RunEvents(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                        }
                    }
                }
            }
            else
            {
                if (preEventCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in preEventCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            RunEvents(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                        }
                    }
                }
            }
        }
        protected virtual void RunEvents(IList<EventHandler<IPipelineMessage, EventArgs>> _handlers, IPipelineMessage input)
        {
            if (_handlers.AnyOrNotNull())
            {
                foreach (var handler in _handlers)
                {
                    if (handler == null)
                    {
                        continue;
                    }

                    Task.Factory.StartNew(() => handler(input, EventArgs.Empty));
                }
            }
        }

        #endregion
    }
}

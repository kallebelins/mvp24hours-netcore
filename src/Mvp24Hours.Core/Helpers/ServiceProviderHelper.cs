using System;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;

        private static Func<object, IServiceProvider> _actionProvider;
        private static object _state;

        /// <summary>
        /// 
        /// </summary>
        public static void SetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionProvider"></param>
        public static void SetProvider(Func<object, IServiceProvider> actionProvider, object state)
        {
            _actionProvider = actionProvider;
            _state = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetService(Type type)
        {
            if (_actionProvider != null)
            {
                return _actionProvider(_state)
                    ?.GetService(type);
            }
            return _serviceProvider?.GetService(type);
        }
    }
}

using System;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;
        private static ActionProvider _actionProvider;
        public delegate void ActionProvider(out IServiceProvider provider);

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
        public static void SetProvider(ActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
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
                _actionProvider(out _serviceProvider);
            }
            return _serviceProvider?.GetService(type);
        }
    }
}

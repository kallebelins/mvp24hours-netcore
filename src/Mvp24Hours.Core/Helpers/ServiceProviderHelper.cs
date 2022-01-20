using System;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;

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
        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetService(Type type)
        {
            return _serviceProvider?.GetService(type);
        }
    }
}

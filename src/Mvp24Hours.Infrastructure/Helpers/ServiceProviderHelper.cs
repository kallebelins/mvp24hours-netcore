using System;

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;
        private static bool? isHttpContext;

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
            if (IsHttpContext)
            {
                return HttpContextHelper.GetContext()?.RequestServices?.GetService(type);
            }
            else
            {
                return _serviceProvider?.GetService(type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsHttpContext
        {
            get
            {
                if (isHttpContext == null)
                {
                    isHttpContext = HttpContextHelper.GetContext() != null;
                }
                return (bool)isHttpContext;
            }
        }
    }
}

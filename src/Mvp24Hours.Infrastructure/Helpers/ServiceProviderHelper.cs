using System;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;
        private static bool? isHttpContext;

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

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

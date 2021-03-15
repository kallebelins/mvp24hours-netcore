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
            if (IsHttpContext)
                return (T)HttpContextHelper.GetContext()?.RequestServices?.GetService(typeof(T));
            else
                return (T)_serviceProvider?.GetService(typeof(T));
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

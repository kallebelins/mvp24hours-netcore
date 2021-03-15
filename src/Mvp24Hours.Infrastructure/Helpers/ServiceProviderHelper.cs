using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            return (T)_serviceProvider?.GetService(typeof(T));
        }
    }
}

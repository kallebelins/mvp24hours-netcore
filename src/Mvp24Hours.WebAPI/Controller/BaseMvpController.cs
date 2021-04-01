using Microsoft.AspNetCore.Mvc;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;

namespace Mvp24Hours.WebAPI.Controller
{
    public abstract class BaseMvpController : ControllerBase
    {
        private INotificationContext notificationContext;
        protected virtual INotificationContext NotificationContext => notificationContext ??= ServiceProviderHelper.GetService<INotificationContext>();

        private IHATEOASContext hateoasContext;
        protected virtual IHATEOASContext HATEOASContext => hateoasContext ??= ServiceProviderHelper.GetService<IHATEOASContext>();

    }
}

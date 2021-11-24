//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Mvc;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;

namespace Mvp24Hours.WebAPI.Controller
{
    public abstract class BaseMvpController : ControllerBase
    {
        private INotificationContext notificationContext;
        protected virtual INotificationContext NotificationContext => notificationContext ??= ServiceProviderHelper.GetService<INotificationContext>();

        private IHateoasContext hateoasContext;
        protected virtual IHateoasContext HateoasContext => hateoasContext ??= ServiceProviderHelper.GetService<IHateoasContext>();
    }
}

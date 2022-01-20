//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Mvc;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Logging;

namespace Mvp24Hours.WebAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseMvpController : ControllerBase
    {
        #region [ Properties ]

        private INotificationContext notificationContext;
        private ILoggingService logging;
        private IHateoasContext hateoasContext;

        /// <summary>
        /// 
        /// </summary>
        protected virtual INotificationContext NotificationContext => notificationContext ??= ServiceProviderHelper.GetService<INotificationContext>();
        /// <summary>
        /// 
        /// </summary>
        protected virtual IHateoasContext HateoasContext => hateoasContext ??= ServiceProviderHelper.GetService<IHateoasContext>();
        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logging ??= LoggingService.GetLoggingService();

        #endregion
    }
}

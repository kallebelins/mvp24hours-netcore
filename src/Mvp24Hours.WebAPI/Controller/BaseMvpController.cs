//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Mvc;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;

namespace Mvp24Hours.WebAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseMvpController : ControllerBase
    {
        #region [ Properties ]

        private readonly INotificationContext notificationContext;
        private readonly ILoggingService logging;
        private readonly IHateoasContext hateoasContext;

        /// <summary>
        /// 
        /// </summary>
        protected virtual INotificationContext NotificationContext => notificationContext;
        /// <summary>
        /// 
        /// </summary>
        protected virtual IHateoasContext HateoasContext => hateoasContext;
        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logging;

        #endregion

        #region [ Ctor ]
        protected BaseMvpController(ILoggingService logging = null, INotificationContext notificationContext = null, IHateoasContext hateoasContext = null)
        {
            this.logging = logging;
            this.notificationContext = notificationContext;
            this.hateoasContext = hateoasContext;
        }
        #endregion
    }
}

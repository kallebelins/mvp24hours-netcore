using Mvp24Hours.Core.ValueObjects.Infrastructure;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Infrastructure.Contexts
{
    /// <summary>
    /// Context that represents a container for in-app notifications
    /// </summary>
    public interface INotificationContext
    {
        /// <summary>
        /// List of notifications
        /// </summary>
        IReadOnlyCollection<Notification> Notifications { get; }
        /// <summary>
        /// Indicates whether there is notification in the context
        /// </summary>
        bool HasNotifications { get; }
        /// <summary>
        /// Indicates whether there are notifications that correspond to business failures
        /// </summary>
        bool HasErrorNotifications { get; }
        /// <summary>
        /// Adds a notification to the context from the key and message reference
        /// </summary>
        /// <param name="key">Reference key</param>
        /// <param name="message">Message for notification</param>
        void AddNotification(string key, string message);
        /// <summary>
        /// Adds a notification to the context
        /// </summary>
        /// <param name="notification">Notification</param>
        void AddNotification(Notification notification);
        /// <summary>
        /// Adds a list of notifications to the context
        /// </summary>
        /// <param name="notifications">Notification list</param>
        void AddNotifications(IEnumerable<Notification> notifications);
    }
}

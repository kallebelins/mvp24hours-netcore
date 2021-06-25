//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Enums;
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
        void Add(string key, string message);
        /// <summary>
        /// Adds a notification to the context from the key and message reference
        /// </summary>
        /// <param name="key">Reference key</param>
        /// <param name="message">Message for notification</param>
        /// <param name="type">Type of feedback to the client</param>
        void Add(string key, string message, MessageType type);
        /// <summary>
        /// Adds a notification to the context
        /// </summary>
        /// <param name="notification">Notification</param>
        void Add(Notification notification);
        /// <summary>
        /// Adds a list of notifications to the context
        /// </summary>
        /// <param name="notifications">Notification list</param>
        void Add(IEnumerable<Notification> notifications);
        /// <summary>
        /// Adds a notification to the context from the key and message reference
        /// </summary>
        /// <param name="condition">Parameter for conditional notification</param>
        /// <param name="key">Reference key</param>
        /// <param name="message">Message for notification</param>
        void AddIfTrue(bool condition, string key, string message);
        /// <summary>
        /// Adds a notification to the context from the key and message reference
        /// </summary>
        /// <param name="condition">Parameter for conditional notification</param>
        /// <param name="key">Reference key</param>
        /// <param name="message">Message for notification</param>
        /// <param name="type">Type of feedback to the client</param>
        void AddIfTrue(bool condition, string key, string message, MessageType type);
        /// <summary>
        /// Adds a notification to the context
        /// </summary>
        /// <param name="condition">Parameter for conditional notification</param>
        /// <param name="notification">Notification</param>
        void AddIfTrue(bool condition, Notification notification);
        /// <summary>
        /// Adds a list of notifications to the context
        /// </summary>
        /// <param name="condition">Parameter for conditional notification</param>
        /// <param name="notifications">Notification list</param>
        void AddIfTrue(bool condition, IEnumerable<Notification> notifications);
    }
}

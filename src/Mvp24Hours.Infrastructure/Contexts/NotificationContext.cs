//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Contexts
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Contexts.INotificationContext"/>
    /// </summary>
    public class NotificationContext : INotificationContext
    {
        #region [ Fields / Properties ]
        private readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();
        public bool HasErrorNotifications => _notifications.Any(x => x.Type == MessageType.Error);
        #endregion

        #region [ Ctor ]
        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }
        #endregion

        #region [ Methods ]

        public void Add(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }
        public void Add(string key, string message, MessageType type)
        {
            _notifications.Add(new Notification(key, message, type));
        }
        public void Add(Notification notification)
        {
            _notifications.Add(notification);
        }
        public void Add(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }
        public void AddIfTrue(bool condition, string key, string message)
        {
            if (condition)
            {
                Add(key, message);
            }
        }
        public void AddIfTrue(bool condition, string key, string message, MessageType type)
        {
            if (condition)
            {
                Add(key, message, type);
            }
        }
        public void AddIfTrue(bool condition, Notification notification)
        {
            if (condition)
            {
                Add(notification);
            }
        }
        public void AddIfTrue(bool condition, IEnumerable<Notification> notifications)
        {
            if (condition)
            {
                Add(notifications);
            }
        }

        #endregion
    }
}

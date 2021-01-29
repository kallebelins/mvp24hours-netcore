using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Contexts
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }
    }
}

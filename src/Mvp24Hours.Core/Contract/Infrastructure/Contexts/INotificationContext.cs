using Mvp24Hours.Core.ValueObjects.Infrastructure;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Infrastructure.Contexts
{
    public interface INotificationContext
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        bool HasNotifications { get; }
        void AddNotification(string key, string message);
        void AddNotification(Notification notification);
        void AddNotifications(IEnumerable<Notification> notifications);
    }
}

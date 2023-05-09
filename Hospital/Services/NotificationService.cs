using System.Collections.Generic;
using System.Linq;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.Services;
public class NotificationService
{
    private readonly NotificationRepository _notificationRepository;

    public NotificationService()
    {
        _notificationRepository = new NotificationRepository();
    }

    public List<Notification> GetAll(string forId)
    {
        return _notificationRepository.GetAll(forId);
    }

    public List<Notification> GetAllUnsent(string forId)
    {
        return GetAll(forId).Where(notification => notification.Sent == false).ToList();
    }

    public void Send(Notification notification)
    {
        _notificationRepository.Add(notification);
    }

    public void MarkSent(Notification notification)
    {
        notification.Sent = true;
        _notificationRepository.Update(notification);
    }
}

using System.Collections.Generic;
using System.Linq;
using Hospital.Models;
using Hospital.Serialization;

namespace Hospital.Repositories;
public class NotificationRepository
{
    private const string FilePath = "../../../Data/notifications.csv";

    public List<Notification> GetAll()
    {
        return Serializer<Notification>.FromCSV(FilePath);
    }

    public List<Notification> GetAll(string forId)
    {
        var allNotifications = Serializer<Notification>.FromCSV(FilePath);

        return allNotifications.Where(notification => notification.ForId == forId).ToList();
    }

    public void Add(Notification notification) 
    {
        var allNotifications = GetAll();
        allNotifications.Add(notification);

        Serializer<Notification>.ToCSV(allNotifications, FilePath);
    }

    public void Update(Notification notification)
    {
        var allNotifications = GetAll();

        var indexToUpdate = allNotifications.FindIndex(n => n.Id == notification.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allNotifications[indexToUpdate] = notification;
        Serializer<Notification>.ToCSV(allNotifications, FilePath);
    }
}

using System.Collections.Generic;
using System.Linq;
using Hospital.Notifications.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers;

namespace Hospital.Notifications.Repositories;

public class NotificationRepository
{
    private const string FilePath = "../../../Data/notifications.csv";

    public List<Notification> GetAll()
    {
        return CsvSerializer<Notification>.FromCSV(FilePath, new NotificationReadMapper());
    }

    public List<Notification> GetAll(string forId)
    {
        var allNotifications = CsvSerializer<Notification>.FromCSV(FilePath, new NotificationReadMapper());

        return allNotifications.Where(notification => notification.ForId == forId).ToList();
    }

    public void Add(Notification notification)
    {
        var allNotifications = GetAll();
        allNotifications.Add(notification);

        CsvSerializer<Notification>.ToCSV(allNotifications, FilePath, new NotificationWriteMapper());
    }

    public void Update(Notification notification)
    {
        var allNotifications = GetAll();

        var indexToUpdate = allNotifications.FindIndex(n => n.Id == notification.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allNotifications[indexToUpdate] = notification;
        CsvSerializer<Notification>.ToCSV(allNotifications, FilePath, new NotificationWriteMapper());
    }
}
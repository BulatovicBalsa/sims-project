using CsvHelper.Configuration;
using Hospital.Notifications.Models;

namespace Hospital.Serialization.Mappers;

public class NotificationReadMapper : ClassMap<Notification>
{
    public NotificationReadMapper()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.ForId).Index(1);
        Map(m => m.Message).Index(2);
        Map(m => m.Sent).Index(3);
        Map(m => m.Prescription).Index(4).TypeConverter<PrescriptionTypeConverter>();
        Map(m => m.NotifyTime).Index(5).TypeConverter<NullableDateTimeConverter>();
    }
}
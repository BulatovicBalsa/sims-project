using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Messaging.Models;

namespace Hospital.Serialization.Mappers;

public class EmailMessageReadMapper : ClassMap<EmailMessage>
{
    public EmailMessageReadMapper()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Sender).Index(1).TypeConverter<PersonDTOConverter>();
        Map(m => m.Recipient).Index(2).TypeConverter<PersonDTOConverter>();
        Map(m => m.Text).Index(3);
        Map(m => m.Timestamp).Index(4).TypeConverter<DateTimeConverter>();
        Map(m => m.Subject).Index(5);
    }
}
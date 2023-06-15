using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Core.Accounts.DTOs;
using Hospital.Core.Messaging.Models;

namespace Hospital.Serialization.Mappers;

public class EmailMessageWriteMapper : ClassMap<EmailMessage>
{
    public EmailMessageWriteMapper()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Sender).Index(1).TypeConverter<PersonDTOConverter>();
        Map(m => m.Recipient).Index(2).TypeConverter<PersonDTOConverter>();
        Map(m => m.Text).Index(3);
        Map(m => m.Timestamp).Index(4).TypeConverter<DateTimeConverter>();
        Map(m => m.Subject).Index(5);
    }
}

public class PersonDTOConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text)) return null;
        var personArgs = text.Split(';');
        if (personArgs.Length != 4) throw new FormatException("Invalid PersonDTO format.");

        var id = personArgs[0].Trim();
        var firstName = personArgs[1].Trim();
        var lastName = personArgs[2].Trim();
        var role = Enum.Parse<Role>(personArgs[3].Trim());

        return new PersonDTO(id, firstName, lastName, role);
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is PersonDTO person) return $"{person.Id};{person.FirstName};{person.LastName};{person.Role}";

        return string.Empty;
    }
}
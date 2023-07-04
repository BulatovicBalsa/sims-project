using System;
using System.Globalization;
using System.Windows.Data;
using Hospital.Injectors;
using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using Hospital.Serialization;

namespace Hospital.Converters;

public class DoctorIdToStringConverter : IValueConverter
{
    private readonly MemberRepository _memberRepository = new MemberRepository(SerializerInjector.CreateInstance<ISerializer<Member>>());

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _memberRepository.GetById(value.ToString()).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using System;
using System.Globalization;
using System.Windows.Data;
using Hospital.Repositories;

namespace Hospital.Converters;

public class DoctorIdToStringConverter : IValueConverter
{
    private readonly MemberRepository _memberRepository = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _memberRepository.GetById(value.ToString()).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
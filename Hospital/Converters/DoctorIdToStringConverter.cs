using System;
using System.Globalization;
using System.Windows.Data;
using Hospital.Repositories.Doctor;

namespace Hospital.Converters;

public class DoctorIdToStringConverter : IValueConverter
{
    private readonly DoctorRepository _doctorRepository = DoctorRepository.Instance;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _doctorRepository.GetById(value.ToString()).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
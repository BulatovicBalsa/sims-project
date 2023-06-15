using System;
using System.Globalization;
using System.Windows.Data;
using Hospital.Injectors;
using Hospital.Serialization;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.Converters;

public class DoctorIdToStringConverter : IValueConverter
{
    private readonly DoctorRepository _doctorRepository = new(SerializerInjector.CreateInstance<ISerializer<Doctor>>());

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _doctorRepository.GetById(value.ToString()).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
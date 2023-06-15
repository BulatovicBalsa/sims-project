﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hospital.PhysicalAssets.Models;

namespace Hospital.Converters;

public class EquipmentToAmountInRoomColorConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var equipment = values[0] as Equipment;
        var room = values[1] as Room;

        if (equipment == null || room == null)
            return Brushes.Black;

        return room.GetAmount(equipment) == 0 ? Brushes.Red : Brushes.Black;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("EquipmentToAmountTextColorConverter is a OneWay converter.");
    }
}
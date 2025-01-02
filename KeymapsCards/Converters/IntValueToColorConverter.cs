using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace KeymapsCards.Converters;

public class IntValueToColorConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 2 && 
            values[0] is int progressValue && 
            values[1] is int maximumValue && 
            maximumValue > 0)
        {
            var thresholdHigh = maximumValue * 0.66;
            var thresholdLow = maximumValue * 0.30;

            if (progressValue > thresholdHigh)
                return Brushes.Green;
            
            if (progressValue >= thresholdLow)
                return Brushes.Yellow;
            
            return Brushes.Red;
        }

        return Brushes.Gray;
    }
    
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
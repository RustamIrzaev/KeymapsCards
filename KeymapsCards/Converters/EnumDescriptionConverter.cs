using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using KeymapsCards.Extensions;

namespace KeymapsCards.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.GetDescription();
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType.IsEnum && value is string description)
        {
            foreach (var enumValue in Enum.GetValues(targetType))
            {
                if ((enumValue as Enum)?.GetDescription() == description)
                {
                    return enumValue;
                }
            }
        }

        return BindingOperations.DoNothing;
    }
}
using System;
using System.ComponentModel;
using System.Reflection;
using KeymapsCards.Attributes;

namespace KeymapsCards.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        
        return attribute?.Description ?? value.ToString();
    }
    
    public static bool IsDisabled(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        
        return field?.GetCustomAttribute<DisabledValueAttribute>() != null;
    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kropka.EasyXaml.Client.Views.Resources.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class InverseBooleanToVisibilityConverter : IValueConverter
{
    #region Enums
    enum Parameters
    {
        Normal, Inverse
    }
    #endregion

    #region Methods
    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        var boolValue = (bool)value!;
        var direction = (Parameters)Enum.Parse(typeof(Parameters), ((string)parameter)!);

        if (direction == Parameters.Inverse)
            return !boolValue ? Visibility.Visible : Visibility.Collapsed;

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    #endregion
}
using System.Windows;
using System.Windows.Controls.Primitives;

namespace UV.EasyXaml.Client.Views.Resources.Controls;

public class CustomToggleButton : ToggleButton
{
    #region Properties
    public string LeftText
    {
        get => GetValue(LeftTextProperty) as string;
        set => SetValue(LeftTextProperty, value);
    }

    public static readonly DependencyProperty LeftTextProperty = DependencyProperty.Register(
        nameof(LeftText), typeof(string), typeof(CustomToggleButton), new FrameworkPropertyMetadata(string.Empty));

    public string RightText
    {
        get => GetValue(RightTextProperty) as string;
        set => SetValue(RightTextProperty, value);
    }

    public static readonly DependencyProperty RightTextProperty = DependencyProperty.Register(
        nameof(RightText), typeof(string), typeof(CustomToggleButton), new FrameworkPropertyMetadata(string.Empty));
    #endregion
}
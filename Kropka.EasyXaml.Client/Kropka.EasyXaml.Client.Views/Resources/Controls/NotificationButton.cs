using System.Windows;
using System.Windows.Controls;

namespace Kropka.EasyXaml.Client.Views.Resources.Controls;

public class NotificationButton : Button
{
    #region Properties
    public bool ShowNotification
    {
        get => GetValue(ShowNotificationProperty) is bool && (bool)GetValue(ShowNotificationProperty);
        set => SetValue(ShowNotificationProperty, value);
    }

    public static readonly DependencyProperty ShowNotificationProperty = DependencyProperty.Register(
        nameof(ShowNotification), typeof(bool), typeof(NotificationButton), new FrameworkPropertyMetadata(default));
    #endregion
}
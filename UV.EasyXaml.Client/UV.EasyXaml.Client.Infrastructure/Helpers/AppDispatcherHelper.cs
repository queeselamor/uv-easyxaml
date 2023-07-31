using System.Windows;

namespace UV.EasyXaml.Client.Infrastructure.Helpers;

public class AppDispatcherHelper
{
    #region Methods
    public static void Invoke(Action action)
    {
        Application.Current.Dispatcher.Invoke(action);
    }

    public static void InvokeException(Action<Exception> action, Exception e)
    {
        Application.Current.Dispatcher.Invoke(action, e);
    }
    #endregion
}
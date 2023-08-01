using System.Windows;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.Views;

public partial class SplashScreenWindow : Window, ISplashScreenWindow
{
    public SplashScreenWindow()
    {
        InitializeComponent();
    }
}

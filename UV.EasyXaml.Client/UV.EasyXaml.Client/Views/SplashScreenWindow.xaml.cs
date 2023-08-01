using System.Windows;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.Views;

public partial class SplashScreenWindow : Window, ISplashScreenView
{
    #region Constructors
    public SplashScreenWindow()
    {
        InitializeComponent();
    }
    #endregion

    #region Properties
    public ISplashScreenViewModel ViewModel => DataContext as ISplashScreenViewModel;
    #endregion

    #region Methods
    public void ShowView()
    {
        Show();
        Activate();
    }
    #endregion
}
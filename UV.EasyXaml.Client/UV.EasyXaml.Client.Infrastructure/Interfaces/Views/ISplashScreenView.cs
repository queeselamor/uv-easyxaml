using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;

namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

public interface ISplashScreenView
{
    #region Properties
    object DataContext { get; set; }
    ISplashScreenViewModel ViewModel { get; }
    #endregion

    #region Methods
    void ShowView();
    #endregion
}
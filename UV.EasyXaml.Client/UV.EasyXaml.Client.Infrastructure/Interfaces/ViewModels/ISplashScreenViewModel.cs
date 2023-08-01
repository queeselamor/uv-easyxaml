using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;

public interface ISplashScreenViewModel
{
    #region Properties
    string InfoMessage { get; set; }
    ISplashScreenView View { get; }
    #endregion
}
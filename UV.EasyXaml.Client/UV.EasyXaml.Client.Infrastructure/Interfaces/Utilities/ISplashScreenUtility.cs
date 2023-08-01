namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Utilities;

public interface ISplashScreenUtility
{
    #region Methods
    void InitializeSplashScreen();
    void ShowSplashScreen();
    void CloseSplashScreen();
    void ShowInfoMessage(string message);
    #endregion
}

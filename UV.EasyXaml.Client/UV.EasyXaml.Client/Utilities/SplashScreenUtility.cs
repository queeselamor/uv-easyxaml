using Prism.Ioc;
using System;
using System.Threading;
using System.Windows.Threading;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Utilities;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;
using UV.EasyXaml.Client.ViewModels;
using UV.EasyXaml.Client.Views;

namespace UV.EasyXaml.Client.Utilities;

public class SplashScreenUtility : ISplashScreenUtility
{
    #region Fields
    private readonly IContainerExtension _containerExtension;
    #endregion

    #region Constructors
    public SplashScreenUtility(IContainerExtension containerExtension)
    {
        _containerExtension = containerExtension;

        RegisterSplashScreen();
    }
    #endregion

    #region Properties
    private AutoResetEvent WaitForCreation { get; set; }
    private Thread SplashScreenThread { get; set; }
    private ISplashScreenViewModel SplashViewModel { get; set; }
    #endregion

    #region Methods
    public void ShowSplashScreen()
    {
        SplashScreenThread.Start();
        WaitForCreation.WaitOne();
    }

    public void CloseSplashScreen()
    {
        SplashScreenWindow splashView = (SplashScreenWindow)SplashViewModel.View;
        splashView.Dispatcher.BeginInvoke((Action)splashView.Close);
    }

    public void InitializeSplashScreen()
    {      
        WaitForCreation = new AutoResetEvent(false);

        ThreadStart showSplash =
            () =>
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                   () =>
                   {
                       SplashViewModel = _containerExtension.Resolve<ISplashScreenViewModel>();
                       SplashViewModel.View.ShowView();
                       WaitForCreation.Set();
                   });
                Dispatcher.Run();
            };

        SplashScreenThread = new Thread(showSplash) { Name = "SplashScreen Thread", IsBackground = true };
        SplashScreenThread.SetApartmentState(ApartmentState.STA);
    }

    public void ShowInfoMessage(string message)
    {
        SplashViewModel.InfoMessage = message;
    }

    private void RegisterSplashScreen()
    {
        _containerExtension.Register<ISplashScreenView, SplashScreenWindow>();
        _containerExtension.Register<ISplashScreenViewModel, SplashScreenViewModel>();
    }
    #endregion
}

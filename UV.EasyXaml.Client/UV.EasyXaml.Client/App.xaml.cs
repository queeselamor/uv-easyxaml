using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using UV.EasyXaml.Client.Abstracts;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;
using UV.EasyXaml.Client.Services;
using UV.EasyXaml.Client.ViewModels;
using UV.EasyXaml.Client.Views;

namespace UV.EasyXaml.Client;

public partial class App
{
    #region Properties
    private ISplashScreenWindow SplashScreenWindow { get; set; }
    #endregion

    #region Methods
    protected override void OnInitialized()
    {
        base.OnInitialized();

        CloseSplashScreen();
    }

    protected override Window CreateShell()
    {
        return (Window)Container.Resolve<IShellView>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IShellView, ShellView>();
        containerRegistry.RegisterSingleton<IShellViewModel, ShellViewModel>();
        containerRegistry.RegisterSingleton<ISplashScreenWindow, SplashScreenWindow>();

        ShowSplashScreen();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<ViewsModule>();
        moduleCatalog.AddModule<ViewModelsModule>();
        moduleCatalog.AddModule<ServicesModule>();
        moduleCatalog.AddModule<AbstractsModule>();
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
        {
            var viewName = viewType.FullName?.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName?.Replace(".Views", ".ViewModels");
            var viewModelName = $"{viewName}Model, {viewAssemblyName}".Replace(".Views.", ".ViewModels.");
            return Type.GetType(viewModelName);
        });
    }

    private void ShowSplashScreen()
    {
        SplashScreenWindow = Container.Resolve<ISplashScreenWindow>();
        SplashScreenWindow.Show();

        Thread.Sleep(1000);
    }

    private void CloseSplashScreen()
    {
        SplashScreenWindow.Close();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        HandleException(e.Exception);
    }

    private void HandleException(Exception e)
    {
        var message = e.InnerException != null ? e.InnerException.Message : e.Message;

        try
        {
            var dialogService = ContainerLocator.Container.Resolve<IDialogService>();

            if (dialogService != null)
            {
                var parameters = new DialogParameters
                {
                    { DialogParameterNameConstants.TitleParameter, ContentConstants.ErrorTitle },
                    { DialogParameterNameConstants.MessageParameter, message },
                    { DialogParameterNameConstants.DialogWindowTypeParameter, DialogWindowType.Information },
                    { DialogParameterNameConstants.ConfirmButtonTitleParameter, ContentConstants.OkButtonTitle },
                    { DialogParameterNameConstants.DeclineButtonTitleParameter, ContentConstants.CancelButtonTitle }
                };

                dialogService.ShowDialog(DialogViewNamesConstants.MainMessageBoxDialogView, parameters, null, DialogViewNamesConstants.MainDialogWindow);
            }
            else
            {
                MessageBox.Show(message, "Error");
            }
        }
        catch
        {
            EmergencyExitFromApplication();
        }
    }

    private void EmergencyExitFromApplication()
    {
        Environment.Exit(0);
    }
    #endregion
}
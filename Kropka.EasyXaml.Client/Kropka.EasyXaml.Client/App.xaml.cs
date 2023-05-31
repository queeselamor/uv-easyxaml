using Kropka.EasyXaml.Client.Views;
using Prism.Ioc;
using System.Windows;
using System.Windows.Threading;
using Kropka.EasyXaml.Client.ViewModels;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Reflection;
using Kropka.EasyXaml.Client.Services;
using Kropka.EasyXaml.Client.Infrastructure.Constants;

namespace Kropka.EasyXaml.Client;

public partial class App
{
    #region Methods
    protected override Window CreateShell()
    {
        return (Window)Container.Resolve<IShellView>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IShellView, ShellView>();
        containerRegistry.RegisterSingleton<IShellViewModel, ShellViewModel>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<ViewsModule>();
        moduleCatalog.AddModule<ViewModelsModule>();
        moduleCatalog.AddModule<ServicesModule>();
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

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        HandleException(e.Exception);
    }

    private void HandleException(Exception e)
    {
        try
        {
            var message = e.InnerException != null ? e.InnerException.Message : e.Message;

            MessageBox.Show(message, ContentConstants.ErrorTitle);
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
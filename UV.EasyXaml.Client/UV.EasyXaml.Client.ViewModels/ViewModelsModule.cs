using Prism.Ioc;
using Prism.Modularity;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Dialog;
using UV.EasyXaml.Client.ViewModels.ViewModels;
using UV.EasyXaml.Client.ViewModels.ViewModels.Dialog;

namespace UV.EasyXaml.Client.ViewModels;

public class ViewModelsModule : IModule
{
    #region Methods
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainViewModel, MainViewModel>();
        containerRegistry.RegisterSingleton<ISingleFileConverterViewModel, SingleFileConverterViewModel>();
        containerRegistry.RegisterSingleton<IWelcomeScreenViewModel, WelcomeScreenViewModel>();
        containerRegistry.RegisterSingleton<IFolderConverterViewModel, FolderConverterViewModel>();
        containerRegistry.RegisterSingleton<IConverterViewModel, ConverterViewModel>();
        containerRegistry.RegisterSingleton<IBusyViewModel, BusyViewModel>();
        containerRegistry.RegisterSingleton<IMainMessageBoxDialogViewModel, MainMessageBoxDialogViewModel>();
    }
    #endregion
}
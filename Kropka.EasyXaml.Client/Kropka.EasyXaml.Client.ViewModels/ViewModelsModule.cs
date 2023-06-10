using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Dialog;
using Kropka.EasyXaml.Client.ViewModels.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Dialog;
using Prism.Ioc;
using Prism.Modularity;

namespace Kropka.EasyXaml.Client.ViewModels;

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
        containerRegistry.RegisterSingleton<IMainMessageBoxDialogViewModel, MainMessageBoxDialogViewModel>();
    }
    #endregion
}
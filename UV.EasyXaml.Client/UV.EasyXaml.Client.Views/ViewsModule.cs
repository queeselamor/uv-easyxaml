using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Dialog;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views.Dialog;
using UV.EasyXaml.Client.Views.Views;
using UV.EasyXaml.Client.Views.Views.Dialog;

namespace UV.EasyXaml.Client.Views;

public class ViewsModule : IModule
{
    #region Methods
    public void OnInitialized(IContainerProvider containerProvider)
    {
        containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNameConstants.ContentRegion, typeof(IMainView));
        containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IConverterView));
        containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNameConstants.BusyRegion, typeof(IBusyView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        RegisterViews(containerRegistry);
        RegisterDialogWindows(containerRegistry);
    }

    private static void RegisterViews(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainView, MainView>();
        containerRegistry.RegisterSingleton<ISingleFileConverterView, SingleFileConverterView>();
        containerRegistry.RegisterSingleton<IFolderConverterView, FolderConverterView>();
        containerRegistry.RegisterSingleton<IConverterView, ConverterView>();
        containerRegistry.RegisterSingleton<IBusyView, BusyView>();
        containerRegistry.RegisterSingleton<IMainMessageBoxDialogView, MainMessageBoxDialogView>();
    }

    private void RegisterDialogWindows(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterDialog<MainMessageBoxDialogView, IMainMessageBoxDialogViewModel>(DialogViewNamesConstants.MainMessageBoxDialogView);
        containerRegistry.RegisterDialogWindow<MainDialogWindow>(DialogViewNamesConstants.MainDialogWindow);
    }
    #endregion
}
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Kropka.EasyXaml.Client.Views.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.Views;

public class ViewsModule : IModule
{
    #region Methods
    public void OnInitialized(IContainerProvider containerProvider)
    {
        containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNameConstants.ContentRegion, typeof(IMainView));
        containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(ISingleFileConverterView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        RegisterViews(containerRegistry);
    }

    private static void RegisterViews(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainView, MainView>();
        containerRegistry.RegisterSingleton<ISingleFileConverterView, SingleFileConverterView>();
    }
    #endregion
}
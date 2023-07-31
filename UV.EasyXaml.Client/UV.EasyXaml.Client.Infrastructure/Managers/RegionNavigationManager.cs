using Prism.Regions;

namespace UV.EasyXaml.Client.Infrastructure.Managers;

public static class RegionNavigationManager
{
    public static void Navigate(IRegionManager regionManager, string regionName, string viewName)
    {
        regionManager?.RequestNavigate(regionName, viewName);
    }

    public static void Navigate(IRegionManager regionManager, string regionName, string viewName, NavigationParameters navigationParameters)
    {
        regionManager?.RequestNavigate(regionName, viewName, navigationParameters);
    }
}
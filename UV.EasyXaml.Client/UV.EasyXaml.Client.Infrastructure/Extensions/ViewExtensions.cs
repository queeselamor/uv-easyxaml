using Prism.Ioc;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views.Base;

namespace UV.EasyXaml.Client.Infrastructure.Extensions;

public static class ViewExtensions
{
    #region Fields
    private static readonly IContainerProvider Container;
    #endregion

    #region Constructors
    static ViewExtensions()
    {
        Container = ContainerLocator.Container.Resolve<IContainerProvider>();
    }
    #endregion

    #region Methods
    public static TView GetView<TView>() where TView : IView
    {
        return Container.Resolve<TView>();
    }

    public static TView SetDataContext<TView>(this TView view) where TView : IView
    {
        var viewModelType = FindViewDataContext(view);

        view.DataContext = Container.Resolve(viewModelType);

        return view;
    }

    private static Type FindViewDataContext<TView>(TView view) where TView : IView
    {
        var assembly = typeof(TView).Assembly;

        return assembly.DefinedTypes.FirstOrDefault(type => type.Name.Equals("I" + view.GetType().Name + "Model"))!;
    }

    public static TView ActivateRegionWithView<TView>(this TView view, string regionName) where TView : IView
    {
        view.SetDataContext();

        var regionManager = Container.Resolve<IRegionManager>();

        if (!regionManager.Regions.ContainsRegionWithName(regionName))
        {
            IRegion region = new Region { Name = regionName };
            regionManager.Regions.Add(region);
        }

        if (!regionManager.Regions[regionName].Views.Contains(view))
        {
            regionManager.AddToRegion(regionName, view);
        }

        return view;
    }
    #endregion
}
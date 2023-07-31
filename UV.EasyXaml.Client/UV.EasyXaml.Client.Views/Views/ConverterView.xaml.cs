using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.Views.Views;

public partial class ConverterView : IConverterView
{
    public ConverterView()
    {
        InitializeComponent();
    }

    public ConverterView(IRegionManager regionManager) : this()
    {
        RegionManager.SetRegionManager(this, regionManager);
    }
}
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.Views.Views;

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
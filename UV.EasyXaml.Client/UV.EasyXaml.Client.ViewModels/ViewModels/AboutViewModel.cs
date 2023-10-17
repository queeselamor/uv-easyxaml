using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Managers;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class AboutViewModel : BaseViewModel, IAboutViewModel
{
    private readonly IRegionManager _regionManager;

    public AboutViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        ReturnBackCommand = new RelayCommand(ReturnBack);
    }

    private void ReturnBack()
    {
        RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView);
    }

    public string Test { get; set; } = "About View";
    public IRelayCommand ReturnBackCommand { get; }
}
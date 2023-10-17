using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Managers;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class AboutViewModel : BaseViewModel, IAboutViewModel
{
    #region Constants
    private const string DonateUrl = "https://ko-fi.com/queeselamor";
    #endregion

    #region Fields
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Constructors
    public AboutViewModel()
    {
        ReturnBackCommand = new RelayCommand(ReturnBack);
        GoToDonateCommand = new RelayCommand(GoToDonate);
    }
    public AboutViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : this()
    {
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
    }
    #endregion

    #region Commands
    public IRelayCommand ReturnBackCommand { get; }
    public IRelayCommand GoToDonateCommand { get; }
    #endregion

    #region Methods
    private void ReturnBack()
    {
        RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView);

        _eventAggregator.GetEvent<AboutClosedEvent>().Publish(true);
    }

    private void GoToDonate()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = DonateUrl, UseShellExecute = true
        });
    }
    #endregion
}
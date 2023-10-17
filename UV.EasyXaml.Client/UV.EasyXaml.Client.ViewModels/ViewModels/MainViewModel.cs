using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Records;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;
using UV.EasyXaml.Client.Infrastructure.Managers;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class MainViewModel : BaseViewModel, IMainViewModel
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    private readonly IRegionManager _regionManager;
    private bool _isBusy;
    private bool _isAboutOpened;

    #endregion

    #region Constructors
    public MainViewModel()
    {
        OpenAboutCommand = new RelayCommand(OpenAbout);
    }

    public MainViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : this()
    {
        _eventAggregator = eventAggregator;
        _regionManager = regionManager;

        SubscribeOnEvents();
    }
    #endregion

    #region Commands
    public IRelayCommand OpenAboutCommand { get; }
    #endregion

    #region Properties
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public bool IsAboutOpened
    {
        get => _isAboutOpened;
        set => SetProperty(ref _isAboutOpened, value);
    }
    #endregion

    #region Methods
    private void SubscribeOnEvents()
    {
        _eventAggregator.GetEvent<IsBusyChangedEvent>().Subscribe(IsBusyChanged);
        _eventAggregator.GetEvent<AboutClosedEvent>().Subscribe(AboutClosed);
    }

    private void IsBusyChanged(IBusyMessage message)
    {
        IsBusy = message.IsBusy;
    }

    private void AboutClosed(bool isClosed)
    {
        if (isClosed)
        {
            IsAboutOpened = false;
        }
    }

    private void OpenAbout()
    {
        var isViewAlreadyRegistered = _regionManager.Regions[RegionNameConstants.MainRegion].Views.Count() > 1;

        if (!isViewAlreadyRegistered)
        {
            _regionManager.RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IAboutView));
        }

        RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.AboutView);

        IsAboutOpened = true;
    }
    #endregion
}
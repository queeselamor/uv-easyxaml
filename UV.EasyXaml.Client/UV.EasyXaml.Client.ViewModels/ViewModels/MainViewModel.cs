using Prism.Events;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Records;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class MainViewModel : BaseViewModel, IMainViewModel
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    private bool _isBusy;
    #endregion

    #region Constructors
    public MainViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        SubscribeOnEvents();
    }
    #endregion

    #region Properties
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
    #endregion

    #region Methods
    private void SubscribeOnEvents()
    {
        _eventAggregator.GetEvent<IsBusyChangedEvent>().Subscribe(IsBusyChanged);
    }

    private void IsBusyChanged(IBusyMessage message)
    {
        IsBusy = message.IsBusy;
    }
    #endregion
}
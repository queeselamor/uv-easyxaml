using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Records;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Prism.Events;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

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
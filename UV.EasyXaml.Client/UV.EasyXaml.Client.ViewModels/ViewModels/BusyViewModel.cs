using Prism.Events;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Records;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class BusyViewModel : BaseViewModel, IBusyViewModel
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    private string _message;
    #endregion

    #region Constructors
    public BusyViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        Message = ContentConstants.LoadingDefaultTitle;

        SubscribeOnEvents();
    }
    #endregion

    #region Properties
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
    #endregion

    #region Methods
    private void SubscribeOnEvents()
    {
        _eventAggregator.GetEvent<IsBusyChangedEvent>().Subscribe(IsBusyChanged);
    }

    private void IsBusyChanged(IBusyMessage message)
    {
        Message = message.Message;
    }
    #endregion
}
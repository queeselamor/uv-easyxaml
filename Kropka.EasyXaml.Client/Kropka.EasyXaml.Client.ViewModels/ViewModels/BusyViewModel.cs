using System.Windows;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Prism.Events;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class BusyViewModel : BaseViewModel, IBusyViewModel
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    private string _message;
    #endregion

    public BusyViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        Message = ContentConstants.LoadingDefaultTitle;

        SubscribeOnEvents();
    }

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

    private void IsBusyChanged(IBusyMessageViewModel message)
    {
        Message = message.Message;
    }
    #endregion
}
using Prism.Events;
using UV.EasyXaml.Client.Abstracts.Records;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services;

namespace UV.EasyXaml.Client.Services.Services;

public class BusyService : IBusyService
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    #endregion


    #region Constructors
    public BusyService(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }
    #endregion

    #region Methods
    public void ChangeBusyState(bool isBusy, string message)
    {
        _eventAggregator.GetEvent<IsBusyChangedEvent>().Publish(new BusyMessage(isBusy, message));
    }
    #endregion

}
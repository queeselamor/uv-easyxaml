using Prism.Events;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Records;

namespace UV.EasyXaml.Client.Infrastructure.Events;

public class IsBusyChangedEvent : PubSubEvent<IBusyMessage>
{
}
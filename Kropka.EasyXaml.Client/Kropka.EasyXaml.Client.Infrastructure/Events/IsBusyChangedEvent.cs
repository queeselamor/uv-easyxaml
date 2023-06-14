using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Records;
using Prism.Events;

namespace Kropka.EasyXaml.Client.Infrastructure.Events;

public class IsBusyChangedEvent : PubSubEvent<IBusyMessage>
{
}
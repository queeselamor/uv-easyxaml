using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Prism.Events;

namespace Kropka.EasyXaml.Client.Infrastructure.Events;

public class IsBusyChangedEvent : PubSubEvent<IBusyMessageViewModel>
{
}
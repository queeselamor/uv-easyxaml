using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Records;

namespace Kropka.EasyXaml.Client.ViewModels.Records;

public record BusyMessage(bool IsBusy, string Message) : IBusyMessage;
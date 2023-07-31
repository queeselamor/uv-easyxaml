using UV.EasyXaml.Client.Infrastructure.Interfaces.Records;

namespace UV.EasyXaml.Client.Abstracts.Records;

public record BusyMessage(bool IsBusy, string Message) : IBusyMessage;
namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Records;

public interface IBusyMessage
{
    #region Properties
    public bool IsBusy { get; }
    public string Message { get; }
    #endregion
}
namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Services;

public interface IBusyService : IService
{
    #region Methods
    void ChangeBusyState(bool isBusy, string message);
    #endregion
}
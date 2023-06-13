using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Base;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;

public interface IBusyMessageViewModel : IViewModel
{
    #region Properties
    public bool IsBusy { get; }
    public string Message { get; }
    #endregion
}
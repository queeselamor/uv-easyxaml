using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;

public class BusyMessageViewModel : IBusyMessageViewModel
{
    #region Properties
    public bool IsBusy { get; set; }
    public string Message { get; set; }
    #endregion
}
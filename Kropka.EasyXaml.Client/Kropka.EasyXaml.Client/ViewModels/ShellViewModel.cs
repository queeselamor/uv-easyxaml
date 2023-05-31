using Kropka.EasyXaml.Client.Infrastructure.Constants;

namespace Kropka.EasyXaml.Client.ViewModels;

public class ShellViewModel : IShellViewModel
{
    #region Properties
    public string Title => ContentConstants.AppTitle;
    #endregion
}
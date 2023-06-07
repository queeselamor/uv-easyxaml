using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Views;
using Prism.Ioc;

namespace Kropka.EasyXaml.Client.ViewModels;

public class ShellViewModel : IShellViewModel
{
    #region Fields
    private readonly IContainerProvider _containerProvider;
    #endregion

    #region Constructors
    public ShellViewModel()
    {
        DragWindowCommand = new RelayCommand(DragWindow);
    }

    public ShellViewModel(IContainerProvider containerProvider) : this()
    {
        _containerProvider = containerProvider;
    }
    #endregion

    #region Properties
    public string Title => ContentConstants.AppTitle;
    #endregion

    #region Commands
    public IRelayCommand DragWindowCommand { get; }
    #endregion

    #region Methods
    private void DragWindow()
    {
        var shell = (Window)_containerProvider.Resolve<IShellView>();
        shell.DragMove();
    }
    #endregion
}
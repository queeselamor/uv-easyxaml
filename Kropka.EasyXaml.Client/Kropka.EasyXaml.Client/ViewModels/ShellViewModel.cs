using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
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
        CloseWindowCommand = new RelayCommand(CloseWindow);
        MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
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
    public IRelayCommand CloseWindowCommand { get; }
    public IRelayCommand MinimizeWindowCommand { get; }
    #endregion

    #region Methods
    private void DragWindow()
    {
        var shell = (Window)_containerProvider.Resolve<IShellView>();
        shell.DragMove();
    }

    private void CloseWindow()
    {
        Application.Current.Shutdown();
    }

    private void MinimizeWindow()
    {
        var shell = (Window)_containerProvider.Resolve<IShellView>();
        shell.WindowState = WindowState.Minimized;
    }
    #endregion
}
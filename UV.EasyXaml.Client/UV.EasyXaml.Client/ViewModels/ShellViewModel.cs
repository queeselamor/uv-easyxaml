using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Prism.Ioc;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.ViewModels;

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
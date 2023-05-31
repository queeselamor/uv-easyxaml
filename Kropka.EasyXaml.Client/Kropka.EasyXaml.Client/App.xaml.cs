using Kropka.EasyXaml.Client.Views;
using Prism.Ioc;
using System.Windows;
using Kropka.EasyXaml.Client.ViewModels;

namespace Kropka.EasyXaml.Client;

public partial class App
{
    protected override Window CreateShell()
    {
        return (Window)Container.Resolve<IShellView>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IShellView, ShellView>();
        containerRegistry.RegisterSingleton<IShellViewModel, ShellViewModel>();
    }
}
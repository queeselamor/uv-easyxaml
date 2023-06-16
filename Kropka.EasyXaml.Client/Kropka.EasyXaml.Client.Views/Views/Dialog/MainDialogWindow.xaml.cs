using System.Windows;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Services.Dialogs;

namespace Kropka.EasyXaml.Client.Views.Views.Dialog;

public partial class MainDialogWindow : IDialogWindow
{
    #region Ctor
    public MainDialogWindow(IShellView shell)
    {
        var window = (Window)shell;

        InitializeComponent();

        Owner = window;
    }
    #endregion

    #region Properties
    public IDialogResult Result { get; set; }
    #endregion
}
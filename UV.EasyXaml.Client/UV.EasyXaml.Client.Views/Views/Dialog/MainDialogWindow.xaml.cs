using System.Windows;
using Prism.Services.Dialogs;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.Views.Views.Dialog;

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
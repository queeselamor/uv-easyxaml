using Prism.Services.Dialogs;

namespace Kropka.EasyXaml.Client.Views.Views.Dialog;

public partial class MainDialogWindow : IDialogWindow
{
    #region Ctor
    public MainDialogWindow()
    {
        InitializeComponent();
    }
    #endregion

    #region Properties
    public IDialogResult Result { get; set; }
    #endregion
}
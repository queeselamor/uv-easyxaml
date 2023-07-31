using CommunityToolkit.Mvvm.Input;
using Prism.Services.Dialogs;
using System;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Dialog;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels.Dialog;

public class MainMessageBoxDialogViewModel : BaseViewModel, IMainMessageBoxDialogViewModel
{
    #region Fields
    private string _title;
    private string _message;
    private DialogWindowType _dialogWindowType;
    private string _confirmButtonTitle;
    private string _declineButtonTitle;
    #endregion

    #region ctor
    public MainMessageBoxDialogViewModel()
    {
        OkCommand = new RelayCommand(Ok);
        CancelCommand = new RelayCommand(Cancel);
    }
    #endregion

    #region Properties
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public DialogWindowType DialogWindowType
    {
        get => _dialogWindowType;
        set => SetProperty(ref _dialogWindowType, value);
    }

    public string ConfirmButtonTitle
    {
        get => _confirmButtonTitle;
        set => SetProperty(ref _confirmButtonTitle, value);
    }

    public string DeclineButtonTitle
    {
        get => _declineButtonTitle;
        set => SetProperty(ref _declineButtonTitle, value);
    }

    public bool IsInformationDialog => DialogWindowType is DialogWindowType.Information;
    public bool IsConfirmationDialog => DialogWindowType is DialogWindowType.Confirmation;
    #endregion

    #region Commands
    public IRelayCommand OkCommand { get; set; }
    public IRelayCommand CancelCommand { get; set; }
    #endregion

    #region Methods
    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        Title = parameters.GetValue<string>(DialogParameterNameConstants.TitleParameter);
        Message = parameters.GetValue<string>(DialogParameterNameConstants.MessageParameter);
        DialogWindowType = parameters.GetValue<DialogWindowType>(DialogParameterNameConstants.DialogWindowTypeParameter);
        ConfirmButtonTitle = parameters.GetValue<string>(DialogParameterNameConstants.ConfirmButtonTitleParameter);
        DeclineButtonTitle = parameters.GetValue<string>(DialogParameterNameConstants.DeclineButtonTitleParameter);
    }

    private void Ok()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
    }

    private void Cancel()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    #endregion

    #region Events
    public event Action<IDialogResult> RequestClose;
    #endregion
}
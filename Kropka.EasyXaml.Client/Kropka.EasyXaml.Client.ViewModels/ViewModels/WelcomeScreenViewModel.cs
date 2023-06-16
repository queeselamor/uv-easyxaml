﻿using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Regions;
using Kropka.EasyXaml.Client.Infrastructure.Managers;
using Prism.Services.Dialogs;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Helpers;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class WelcomeScreenViewModel : BaseViewModel, IWelcomeScreenViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IRegionManager _regionManager;
    private readonly IDialogService _dialogService;
    private readonly IImageTransformationManager _imageTransformationManager;
    #endregion

    #region Constructors
    public WelcomeScreenViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFileAsync);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);
    }

    public WelcomeScreenViewModel(IFileService fileService, IRegionManager regionManager, IDialogService dialogService, IImageTransformationManager imageTransformationManager) : this()
    {
        _fileService = fileService;
        _regionManager = regionManager;
        _dialogService = dialogService;
        _imageTransformationManager = imageTransformationManager;

        Task.Run(CheckConvertersTransformationFiles);
    }
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IAsyncRelayCommand PickFolderCommand { get; }
    #endregion

    #region Methods

    private async Task CheckConvertersTransformationFiles()
    {
        var isSvgToXamlConvertersExist = await _imageTransformationManager.CheckTransformationFileExistsAsync(ConverterType.SvgToXaml);

        if (!isSvgToXamlConvertersExist)
        {
            ShowError(ContentConstants.SvgTransformationsFileMissingError);
        }
    }

    private void ShowError(string message)
    {
        if (_dialogService == null) return;

        AppDispatcherHelper.Invoke(() =>
        {
            var parameters = new DialogParameters
            {
                { DialogParameterNameConstants.TitleParameter, ContentConstants.WarningTitle },
                { DialogParameterNameConstants.MessageParameter, message },
                { DialogParameterNameConstants.DialogWindowTypeParameter, DialogWindowType.Information },
                { DialogParameterNameConstants.ConfirmButtonTitleParameter, ContentConstants.OkButtonTitle },
                { DialogParameterNameConstants.DeclineButtonTitleParameter, ContentConstants.CancelButtonTitle }
            };

            _dialogService.ShowDialog(DialogViewNamesConstants.MainMessageBoxDialogView, parameters, null, DialogViewNamesConstants.MainDialogWindow);
        });
    }

    private async Task PickFileAsync()
    {
        var filePath = await _fileService.PickFilePathAsync();

        if (filePath != string.Empty)
        {
            _regionManager.RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IConverterView));

            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FilePath, filePath
                }
            };

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView, parameters);
        }
    }

    private async Task PickFolderAsync()
    {
        var folderPath = await _fileService.PickFolderAsync();

        if (folderPath != string.Empty)
        {
            _regionManager.RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IConverterView));

            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FolderPath, folderPath
                }
            };

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView, parameters);
        }
    }
    #endregion
}
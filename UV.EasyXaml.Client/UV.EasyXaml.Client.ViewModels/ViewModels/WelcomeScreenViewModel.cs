using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Services.Dialogs;
using Prism.Events;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Helpers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;
using UV.EasyXaml.Client.Infrastructure.Managers;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class WelcomeScreenViewModel : BaseViewModel, IWelcomeScreenViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IRegionManager _regionManager;
    private readonly IDialogService _dialogService;
    private readonly IImageTransformationManager _imageTransformationManager;
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Constructors
    public WelcomeScreenViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFileAsync);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);
    }

    public WelcomeScreenViewModel(IFileService fileService, IRegionManager regionManager, IDialogService dialogService,
        IImageTransformationManager imageTransformationManager, IEventAggregator eventAggregator) : this()
    {
        _fileService = fileService;
        _regionManager = regionManager;
        _dialogService = dialogService;
        _imageTransformationManager = imageTransformationManager;
        _eventAggregator = eventAggregator;

        RegisterEvents();

        Task.Run(CheckConvertersTransformationFiles);
    }
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IAsyncRelayCommand PickFolderCommand { get; }
    #endregion

    #region Methods
    private void RegisterEvents()
    {
        _eventAggregator.GetEvent<FileDroppedEvent>().Subscribe(OnFileDropped);
        _eventAggregator.GetEvent<FolderDroppedEvent>().Subscribe(OnFolderDropped);
    }

    private async void OnFileDropped(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        var isSvg = await _fileService.CheckFileExtension(filePath, ExtensionFilterConstants.SvgExtensionFilter);

        if (isSvg)
        {
            NavigateToFileMode(filePath);
        }
    }

    private async void OnFolderDropped(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        var isFolder = await _fileService.CheckIsFolder(folderPath);

        if (isFolder)
        {
            NavigateToFolderMode(folderPath);
        }
    }

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
            NavigateToFileMode(filePath);
        }
    }

    private void NavigateToFileMode(string filePath)
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

    private async Task PickFolderAsync()
    {
        var folderPath = await _fileService.PickFolderAsync();

        if (folderPath != string.Empty)
        {
            NavigateToFolderMode(folderPath);
        }
    }

    private void NavigateToFolderMode(string folderPath)
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
    #endregion
}
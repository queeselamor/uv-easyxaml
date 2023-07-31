using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Regions;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Events;
using UV.EasyXaml.Client.Infrastructure.Helpers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Models;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using UV.EasyXaml.Client.ViewModels.ViewModels.Base;
using UV.EasyXaml.Client.ViewModels.ViewModels.Model;

namespace UV.EasyXaml.Client.ViewModels.ViewModels;

public class FolderConverterViewModel : BaseViewModel, IFolderConverterViewModel
{
    #region Fields
    private ObservableCollection<IConverterItemViewModel> _converterItems;
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
    private readonly IBusyService _busyService;
    private readonly IEventAggregator _eventAggregator;
    private string _chosenFolderPath;
    private bool _showCopyNotification;
    private bool _showSaveNotification;
    private bool _showSaveAllNotification;
    private IConverterItemViewModel _selectedConverterItem;
    private bool _isSelectedAll;
    private string _previousFolderPath = string.Empty;
    #endregion

    #region Constructors
    public FolderConverterViewModel()
    {
        ConverterItems = new ObservableCollection<IConverterItemViewModel>();

        SaveFilesCommand = new AsyncRelayCommand(SaveFilesAsync, CanSaveFiles);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);

        SelectAllCommand = new RelayCommand(SelectAll);
        CopySelectedContentCommand = new RelayCommand(CopySelectedContent);
        SaveSelectedFileCommand = new AsyncRelayCommand(SaveSelectedFile);
        ChangeShowingContentCommand = new RelayCommand(ChangeShowingContent);

        CanUpdateStates = true;
    }

    public FolderConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager,
        IBusyService busyService, IEventAggregator eventAggregator) : this()
    {
        _fileService = fileService;
        _imageTransformationManager = imageTransformationManager;
        _busyService = busyService;
        _eventAggregator = eventAggregator;

        RegisterEvents();
    }
    #endregion

    #region Properties
    public ObservableCollection<IConverterItemViewModel> ConverterItems
    {
        get => _converterItems;
        set => SetProperty(ref _converterItems, value);
    }

    public IConverterItemViewModel SelectedConverterItem
    {
        get => _selectedConverterItem;
        set
        {
            if (SetProperty(ref _selectedConverterItem, value))
            {
                RaisePropertyChanged(nameof(SelectedConverterItem.ResultContent));
            }
        }
    }

    public string ChosenFolderPath
    {
        get => _chosenFolderPath;
        set => SetProperty(ref _chosenFolderPath, value);
    }

    public bool ShowCopyNotification
    {
        get => _showCopyNotification;
        set => SetProperty(ref _showCopyNotification, value);
    }

    public bool ShowSaveNotification
    {
        get => _showSaveNotification;
        set => SetProperty(ref _showSaveNotification, value);
    }

    public bool ShowSaveAllNotification
    {
        get => _showSaveAllNotification;
        set => SetProperty(ref _showSaveAllNotification, value);
    }

    public bool IsSelectedAll
    {
        get => _isSelectedAll;
        set => SetProperty(ref _isSelectedAll, value);
    }

    public bool CanUpdateStates { get; set; }
    #endregion

    #region Commands
    public IAsyncRelayCommand SaveFilesCommand { get; }
    public IAsyncRelayCommand PickFolderCommand { get; }
    public IRelayCommand CopySelectedContentCommand { get; }
    public IAsyncRelayCommand SaveSelectedFileCommand { get; }
    public IRelayCommand SelectAllCommand { get; }
    public IRelayCommand ChangeShowingContentCommand { get; }
    #endregion

    #region Methods
    private void RegisterEvents()
    {
        _eventAggregator.GetEvent<FolderDroppedFolderModeEvent>().Subscribe(OnFolderDropped);
    }

    private async void OnFolderDropped(string folderPath)
    {
        _previousFolderPath = ChosenFolderPath;

        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        var isFolder = await _fileService.CheckIsFolder(folderPath);

        if (!isFolder) return;

        if (CheckCurrentFolderPath(folderPath))
        {
            return;
        }

        _busyService.ChangeBusyState(true, ContentConstants.ConvertingTitle);

        ChosenFolderPath = folderPath;

        await ConvertFolderAsync(folderPath);
    }

    private bool CheckCurrentFolderPath(string folderPath)
    {
        if (ChosenFolderPath is null)
        {
            return false;
        }

        return ChosenFolderPath == folderPath;
    }

    private void ChangeShowingContent()
    {
        if (SelectedConverterItem is null)
        {
            return;
        }

        if (SelectedConverterItem.IsShowingDrawingContent)
        {
            SelectedConverterItem.ShowingContent = SelectedConverterItem.AlternativeResultContent;

            return;
        }

        SelectedConverterItem.ShowingContent = SelectedConverterItem.ResultContent;
    }

    private void SelectAll()
    {
        CanUpdateStates = false;

        foreach (var converterItemViewModel in ConverterItems)
        {
            converterItemViewModel.IsSelectedForSave = IsSelectedAll;
        }

        CanUpdateStates = true;

        SaveFilesCommand.NotifyCanExecuteChanged();
    }

    public void UpdateStates()
    {
        IsSelectedAll = ConverterItems.All(x => x.IsSelectedForSave);

        SaveFilesCommand.NotifyCanExecuteChanged();
    }

    private void SelectConverterItem()
    {
        if (ConverterItems.Count > 0)
        {
            SelectedConverterItem = ConverterItems[0];
        }
    }

    private async Task PickFolderAsync()
    {
        _previousFolderPath = ChosenFolderPath;

        var folderPath = await _fileService.PickFolderAsync();

        if (CheckCurrentFolderPath(folderPath))
        {
            return;
        }

        ChosenFolderPath = folderPath;

        _busyService.ChangeBusyState(true, ContentConstants.ConvertingTitle);

        await ConvertFolderAsync(folderPath);

        IsSelectedAll = false;
    }

    private void CopySelectedContent()
    {
        if (SelectedConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(SelectedConverterItem.ShowingContent))
        {
            return;
        }

        Clipboard.SetText(SelectedConverterItem.ShowingContent);

        Task.Run(DisplayCopyNotification);
    }

    private async Task DisplayCopyNotification()
    {
        ShowCopyNotification = true;

        await Task.Delay(2000);

        ShowCopyNotification = false;
    }

    private async Task SaveSelectedFile()
    {
        if (SelectedConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(SelectedConverterItem.ShowingContent))
        {
            return;
        }

        var filePath = await _fileService.SaveFileAsync(SelectedConverterItem.ShowingContent, SelectedConverterItem.SourcePath);

        if (filePath != string.Empty)
        {
            SelectedConverterItem.ResultPath = filePath;

            Task.Run(DisplaySaveNotification);
        }
    }

    private async Task DisplaySaveNotification()
    {
        ShowSaveNotification = true;

        await Task.Delay(2000);

        ShowSaveNotification = false;
    }

    private async Task SaveFilesAsync()
    {
        if (ConverterItems.Count == 0)
        {
            return;
        }

        var folderPath = await _fileService.PickFolderAsync();

        if (folderPath == string.Empty)
        {
            return;
        }

        try
        {
            _busyService.ChangeBusyState(true, ContentConstants.SavingTitle);

            foreach (var converterItemViewModel in ConverterItems.Where(x => x.IsSelectedForSave))
            {
                var filePath = await _fileService.SaveFileAsync(converterItemViewModel.ShowingContent, converterItemViewModel.SourcePath, folderPath);

                converterItemViewModel.ResultPath = filePath;
            }

            Task.Run(DisplaySaveAllNotification);
        }
        finally
        {
            _busyService.ChangeBusyState(false, string.Empty);
        }
    }

    private bool CanSaveFiles()
    {
        return ConverterItems.Any(x => x.IsSelectedForSave);
    }

    private async Task DisplaySaveAllNotification()
    {
        ShowSaveAllNotification = true;

        await Task.Delay(2000);

        ShowSaveAllNotification = false;
    }

    private async Task ConvertFolderAsync(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            _busyService.ChangeBusyState(false, string.Empty);

            return;
        }

        var filesCount = await GetFilePathsAsync(folderPath);

        if (filesCount == 0)
        {
            _busyService.ChangeBusyState(false, string.Empty);

            ChosenFolderPath = _previousFolderPath;

            throw new Exception(ContentConstants.FolderIsEmptyError);
        }

        await ConvertItemsAsync();

        SelectConverterItem();
    }

    private async Task<int> GetFilePathsAsync(string folderPath)
    {
        var filePaths = await _fileService.GetFilePathsAsync(folderPath);

        var paths = filePaths.ToList();

        if (!paths.Any())
        {
            return 0;
        }

        CreateConverterItems(paths);

        return paths.Count;
    }

    private async void CreateConverterItems(IEnumerable<string> filePaths)
    {
        ConverterItems.Clear();

        foreach (var filePath in filePaths)
        {
            var converterItem = new ConverterItemViewModel(this, ConverterType.SvgToXaml, filePath)
            {
                SourceFileName = await _fileService.GetFileNameAsync(filePath)
            };

            ConverterItems.Add(converterItem);
        }
    }

    private async Task ConvertItemsAsync()
    {
        try
        {
            if (ConverterItems is null)
            {
                return;
            }

            foreach (var converterItemViewModel in ConverterItems)
            {
                var transformContentResponse = await _imageTransformationManager.TransformAsync(converterItemViewModel.ConverterType, converterItemViewModel.SourcePath);

                if (transformContentResponse is not IXamlConverterResponse response)
                {
                    converterItemViewModel.ResultContent = string.Empty;

                    continue;
                }

                if (response.IsSuccessConvertToCanvas)
                {
                    var canvasContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, response.CanvasContent);

                    converterItemViewModel.ResultContent = canvasContent;
                    converterItemViewModel.IsShowingDrawingContent = false;
                }
                else
                {
                    converterItemViewModel.IsShowingDrawingContent = true;
                }

                if (response.IsSuccessConvertToDrawingGroup)
                {
                    var drawingGroupContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, response.DrawingGroupContent);

                    converterItemViewModel.AlternativeResultContent = drawingGroupContent;

                    AppDispatcherHelper.Invoke(() =>
                    {
                        var brush = _imageTransformationManager.DetermineBackground(ConverterType.SvgToXaml, drawingGroupContent);

                        converterItemViewModel.PreviewBackground = brush;
                    });
                }
                else
                {
                    converterItemViewModel.IsShowingDrawingContent = false;
                }

                if (response.IsSuccessConvertToCanvas && response.IsSuccessConvertToDrawingGroup)
                {
                    converterItemViewModel.HasTwoContentVariants = true;
                }

                if (converterItemViewModel.IsShowingDrawingContent)
                {
                    converterItemViewModel.ShowingContent = converterItemViewModel.AlternativeResultContent;

                    continue;
                }

                converterItemViewModel.ShowingContent = converterItemViewModel.ResultContent;
            }
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            _busyService.ChangeBusyState(false, string.Empty);
        }
    }
    #endregion

    #region Navigation
    public async void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        if (navigationContext.Parameters[NavigationParameterConstants.FolderPath] is not string folderPath)
        {
            return;
        }

        _busyService.ChangeBusyState(true, ContentConstants.ConvertingTitle);

        _previousFolderPath = ChosenFolderPath;

        if (CheckCurrentFolderPath(folderPath))
        {
            return;
        }

        ChosenFolderPath = folderPath;

        await ConvertFolderAsync(folderPath);
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
    #endregion
}
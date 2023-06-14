using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.Records;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;
using Prism.Events;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class FolderConverterViewModel : BaseViewModel, IFolderConverterViewModel
{
    #region Fields
    private ObservableCollection<IConverterItemViewModel> _converterItems;
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
    private readonly IEventAggregator _eventAggregator;
    private string _chosenFolderPath;
    private bool _showCopyNotification;
    private bool _showSaveNotification;
    private IConverterItemViewModel _selectedConverterItem;

    #endregion

    #region Constructors
    public FolderConverterViewModel()
    {
        ConverterItems = new ObservableCollection<IConverterItemViewModel>();

        SaveFilesCommand = new AsyncRelayCommand(SaveFilesAsync);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);

        CopySelectedContentCommand = new RelayCommand(CopySelectedContent);
        SaveSelectedFileCommand = new AsyncRelayCommand(SaveSelectedFile);
    }

    public FolderConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager, IEventAggregator eventAggregator) : this()
    {
        _fileService = fileService;
        _imageTransformationManager = imageTransformationManager;
        _eventAggregator = eventAggregator;
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
    #endregion

    #region Commands
    public IAsyncRelayCommand SaveFilesCommand { get; }
    public IAsyncRelayCommand PickFolderCommand { get; }
    public IRelayCommand CopySelectedContentCommand { get; }
    public IAsyncRelayCommand SaveSelectedFileCommand { get; }
    #endregion

    #region Methods
    private void SelectConverterItem()
    {
        if (ConverterItems.Count > 0)
        {
            SelectedConverterItem = ConverterItems[0];
        }
    }

    private async Task PickFolderAsync()
    {
        var folderPath = await _fileService.PickFolderAsync();

        ChosenFolderPath = folderPath;

        _eventAggregator.GetEvent<IsBusyChangedEvent>().Publish(new BusyMessage(true, ContentConstants.ConvertingTitle));

        await ConvertFolderAsync(folderPath);
    }

    private void CopySelectedContent()
    {
        if (SelectedConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(SelectedConverterItem.ResultContent))
        {
            return;
        }

        Clipboard.SetText(SelectedConverterItem.ResultContent);

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

        if (string.IsNullOrEmpty(SelectedConverterItem.ResultContent))
        {
            return;
        }

        var filePath = await _fileService.SaveFileAsync(SelectedConverterItem.ResultContent, SelectedConverterItem.SourcePath);

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

        foreach (var converterItemViewModel in ConverterItems.Where(x => x.IsSelected))
        {
            var filePath = await _fileService.SaveFileAsync(converterItemViewModel.ResultContent, converterItemViewModel.SourcePath, folderPath);

            converterItemViewModel.ResultPath = filePath;
        }
    }

    private async Task ConvertFolderAsync(string folderPath)
    {
        await GetFilePathsAsync(folderPath);
        await ConvertItemsAsync();

        SelectConverterItem();
    }

    private async Task GetFilePathsAsync(string folderPath)
    {
        var filePaths = await _fileService.GetFilePathsAsync(folderPath);

        CreateConverterItems(filePaths);
    }

    private async void CreateConverterItems(IEnumerable<string> filePaths)
    {
        ConverterItems.Clear();

        foreach (var filePath in filePaths)
        {
            var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath)
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

                var transformContent = response.IsSuccessConvertToCanvas ? response.CanvasContent : response.DrawingGroupContent;

                var resultContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, transformContent);

                converterItemViewModel.ResultContent = resultContent;
            }
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            _eventAggregator.GetEvent<IsBusyChangedEvent>().Publish(new BusyMessage(false, string.Empty));
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

        _eventAggregator.GetEvent<IsBusyChangedEvent>().Publish(new BusyMessage(true, ContentConstants.ConvertingTitle));

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
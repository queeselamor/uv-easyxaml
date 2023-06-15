﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class FolderConverterViewModel : BaseViewModel, IFolderConverterViewModel
{
    #region Fields
    private ObservableCollection<IConverterItemViewModel> _converterItems;
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
    private readonly IBusyService _busyService;
    private string _chosenFolderPath;
    private bool _showCopyNotification;
    private bool _showSaveNotification;
    private IConverterItemViewModel _selectedConverterItem;
    private bool _isSelectedAll;
    #endregion

    #region Constructors
    public FolderConverterViewModel()
    {
        ConverterItems = new ObservableCollection<IConverterItemViewModel>();

        SaveFilesCommand = new AsyncRelayCommand(SaveFilesAsync);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);

        SelectAllCommand = new RelayCommand(SelectAll);
        CopySelectedContentCommand = new RelayCommand(CopySelectedContent);
        SaveSelectedFileCommand = new AsyncRelayCommand(SaveSelectedFile);
        ChangeShowingContentCommand = new RelayCommand(ChangeShowingContent);
    }

    public FolderConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager, IBusyService busyService) : this()
    {
        _fileService = fileService;
        _imageTransformationManager = imageTransformationManager;
        _busyService = busyService;
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

    public bool IsSelectedAll
    {
        get => _isSelectedAll;
        set => SetProperty(ref _isSelectedAll, value);
    }
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
        foreach (var converterItemViewModel in ConverterItems)
        {
            converterItemViewModel.IsSelectedForSave = IsSelectedAll;
        }
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
        var folderPath = await _fileService.PickFolderAsync();

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

        foreach (var converterItemViewModel in ConverterItems.Where(x => x.IsSelectedForSave))
        {
            var filePath = await _fileService.SaveFileAsync(converterItemViewModel.ResultContent, converterItemViewModel.SourcePath, folderPath);

            converterItemViewModel.ResultPath = filePath;
        }
    }

    private async Task ConvertFolderAsync(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            _busyService.ChangeBusyState(false, string.Empty);

            return;
        }

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
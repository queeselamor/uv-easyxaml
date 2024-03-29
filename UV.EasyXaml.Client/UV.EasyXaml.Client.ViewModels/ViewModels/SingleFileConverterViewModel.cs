﻿using System;
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

public class SingleFileConverterViewModel : BaseViewModel, ISingleFileConverterViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
    private readonly IBusyService _busyService;
    private readonly IEventAggregator _eventAggregator;
    private IConverterItemViewModel _currentConverterItem;
    private bool _showCopyNotification;
    private bool _showSaveNotification;
    #endregion

    #region Constructors
    public SingleFileConverterViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFileAsync);
        CopyContentCommand = new RelayCommand(CopyContent);
        SaveFileCommand = new AsyncRelayCommand(SaveFileAsync);
        ChangeShowingContentCommand = new RelayCommand(ChangeShowingContent);
    }

    public SingleFileConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager,
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
    public IConverterItemViewModel CurrentConverterItem
    {
        get => _currentConverterItem;
        set => SetProperty(ref _currentConverterItem, value);
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

    public bool CanUpdateStates { get; set; }
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IRelayCommand CopyContentCommand { get; }
    public IAsyncRelayCommand SaveFileCommand { get; }
    public IRelayCommand ChangeShowingContentCommand { get; }
    #endregion

    #region Methods
    private void RegisterEvents()
    {
        _eventAggregator.GetEvent<FileDroppedSingleModeEvent>().Subscribe(OnFileDropped);
    }

    private async void OnFileDropped(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        var isSvg = await _fileService.CheckFileExtension(filePath, ExtensionFilterConstants.SvgExtensionFilter);

        if (!isSvg) return;

        if (CheckCurrentFilePath(filePath))
        {
            return;
        }

        CreateConverterItem(filePath);

        await ConvertAsync();
    }

    private bool CheckCurrentFilePath(string filePath)
    {
        if (CurrentConverterItem is null)
        {
            return false;
        }

        return CurrentConverterItem.SourcePath == filePath;
    }

    private void ChangeShowingContent()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        if (CurrentConverterItem.IsShowingDrawingContent)
        {
            CurrentConverterItem.ShowingContent = CurrentConverterItem.AlternativeResultContent;

            return;
        }

        CurrentConverterItem.ShowingContent = CurrentConverterItem.ResultContent;
    }

    public void UpdateStates()
    {
    }

    private async Task PickFileAsync()
    {
        var filePath = await _fileService.PickFilePathAsync();

        if (filePath != string.Empty)
        {
            if (CheckCurrentFilePath(filePath))
            {
                return;
            }

            CreateConverterItem(filePath);

            _busyService.ChangeBusyState(true, ContentConstants.ConvertingTitle);

            await ConvertAsync();
        }
    }

    private async void CreateConverterItem(string filePath)
    {
        var converterItem = new ConverterItemViewModel(this, ConverterType.SvgToXaml, filePath)
        {
            SourceFileName = await _fileService.GetFileNameAsync(filePath)
        };

        CurrentConverterItem = converterItem;
    }

    private async Task ConvertAsync()
    {
        try
        {
            if (CurrentConverterItem is null)
            {
                return;
            }

            var transformContentResponse = await _imageTransformationManager.TransformAsync(CurrentConverterItem.ConverterType, CurrentConverterItem.SourcePath);

            if (transformContentResponse is not IXamlConverterResponse response)
            {
                CurrentConverterItem.ResultContent = string.Empty;

                return;
            }

            if (response.IsSuccessConvertToCanvas)
            {
                var canvasContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, response.CanvasContent);

                CurrentConverterItem.ResultContent = canvasContent;
                CurrentConverterItem.IsShowingDrawingContent = false;
            }
            else
            {
                CurrentConverterItem.IsShowingDrawingContent = true;
            }

            if (response.IsSuccessConvertToDrawingGroup)
            {
                var drawingGroupContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, response.DrawingGroupContent);

                CurrentConverterItem.AlternativeResultContent = drawingGroupContent;

                AppDispatcherHelper.Invoke(() =>
                {
                    var brush = _imageTransformationManager.DetermineBackground(ConverterType.SvgToXaml, drawingGroupContent);

                    CurrentConverterItem.PreviewBackground = brush;
                });
            }
            else
            {
                CurrentConverterItem.IsShowingDrawingContent = false;
            }

            if (response.IsSuccessConvertToCanvas && response.IsSuccessConvertToDrawingGroup)
            {
                CurrentConverterItem.HasTwoContentVariants = true;
            }

            ChangeShowingContent();
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            _busyService.ChangeBusyState(false, string.Empty);
        }
    }

    private void CopyContent()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(CurrentConverterItem.ShowingContent))
        {
            return;
        }

        Clipboard.SetText(CurrentConverterItem.ShowingContent);

        Task.Run(DisplayCopyNotification);
    }

    private async Task DisplayCopyNotification()
    {
        ShowCopyNotification = true;

        await Task.Delay(2000);

        ShowCopyNotification = false;
    }

    private async Task SaveFileAsync()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(CurrentConverterItem.ShowingContent))
        {
            return;
        }

        var filePath = await _fileService.SaveFileAsync(CurrentConverterItem.ShowingContent, CurrentConverterItem.SourcePath);

        if (filePath != string.Empty)
        {
            CurrentConverterItem.ResultPath = filePath;

            Task.Run(DisplaySaveNotification);
        }
    }

    private async Task DisplaySaveNotification()
    {
        ShowSaveNotification = true;

        await Task.Delay(2000);

        ShowSaveNotification = false;
    }
    #endregion

    #region Navigation
    public async void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        _busyService.ChangeBusyState(true, ContentConstants.ConvertingTitle);

        var filePath = navigationContext.Parameters[NavigationParameterConstants.FilePath] as string;

        if (CheckCurrentFilePath(filePath))
        {
            return;
        }

        CreateConverterItem(filePath);

        await ConvertAsync();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        CurrentConverterItem = null;
    }
    #endregion
}
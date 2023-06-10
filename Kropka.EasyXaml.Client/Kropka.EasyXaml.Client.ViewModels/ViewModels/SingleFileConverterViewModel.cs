﻿using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class SingleFileConverterViewModel : BaseViewModel, ISingleFileConverterViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
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
    }

    public SingleFileConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager) : this()
    {
        _fileService = fileService;
        _imageTransformationManager = imageTransformationManager;
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
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IRelayCommand CopyContentCommand { get; }
    public IAsyncRelayCommand SaveFileCommand { get; }
    #endregion

    #region Methods
    private async Task PickFileAsync()
    {
        var filePath = await _fileService.PickFilePathAsync();

        if (filePath != string.Empty)
        {
            CreateConverterItem(filePath);

            await ConvertAsync();
        }
    }

    private async void CreateConverterItem(string filePath)
    {
        var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath)
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

            var transformContent = await _imageTransformationManager.TransformAsync(CurrentConverterItem.ConverterType, CurrentConverterItem.SourcePath);
            var resultContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, transformContent);

            CurrentConverterItem.ResultContent = resultContent;
        }
        catch (Exception ex)
        {
            //TODO: Hide elements and clear content

            throw;
        }
    }

    private void CopyContent()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(CurrentConverterItem.ResultContent))
        {
            return;
        }

        Clipboard.SetText(CurrentConverterItem.ResultContent);

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

        if (string.IsNullOrEmpty(CurrentConverterItem.ResultContent))
        {
            return;
        }

        var filePath = await _fileService.SaveFileAsync(CurrentConverterItem.ResultContent, CurrentConverterItem.SourcePath);

        CurrentConverterItem.ResultPath = filePath;

        Task.Run(DisplaySaveNotification);
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

        var filePath = navigationContext.Parameters[NavigationParameterConstants.FilePath] as string;

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
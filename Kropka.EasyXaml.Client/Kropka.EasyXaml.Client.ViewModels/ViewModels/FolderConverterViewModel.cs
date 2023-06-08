using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

public class FolderConverterViewModel : BaseViewModel, IFolderConverterViewModel
{
    #region Fields
    private ObservableCollection<IConverterItemViewModel> _converterItems;
    private readonly IFileService _fileService;
    private readonly IImageTransformationManager _imageTransformationManager;
    #endregion

    #region Constructors
    public FolderConverterViewModel()
    {
        ConverterItems = new ObservableCollection<IConverterItemViewModel>();
    }

    public FolderConverterViewModel(IFileService fileService, IImageTransformationManager imageTransformationManager) : this()
    {
        _fileService = fileService;
        _imageTransformationManager = imageTransformationManager;
    }
    #endregion

    #region Properties
    public ObservableCollection<IConverterItemViewModel> ConverterItems
    {
        get => _converterItems;
        set => SetProperty(ref _converterItems, value);
    }
    #endregion

    #region Methods

    private async Task ConvertFolderAsync(string folderPath)
    {
        await GetFilePathsAsync(folderPath);
        await ConvertItemsAsync();
    }

    private async Task GetFilePathsAsync(string folderPath)
    {
        var filePaths = await _fileService.GetFilePathsAsync(folderPath);

        CreateConverterItems(filePaths);
    }

    private void CreateConverterItems(IEnumerable<string> filePaths)
    {
        ConverterItems.Clear();

        foreach (var filePath in filePaths)
        {
            var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath);

            ConverterItems.Add(converterItem);
        }
    }

    private async Task ConvertItemsAsync()
    {
        if (ConverterItems is null)
        {
            return;
        }

        foreach (var converterItemViewModel in ConverterItems)
        {
            var transformContent = await _imageTransformationManager.TransformAsync(converterItemViewModel.ConverterType, converterItemViewModel.SourcePath);
            var resultContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, transformContent);

            converterItemViewModel.ResultContent = resultContent;
        }
    }
    #endregion

    #region Navigation
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        if (navigationContext.Parameters[NavigationParameterConstants.FolderPath] is not string folderPath)
        {
            return;
        }

        Task.Run(() => ConvertFolderAsync(folderPath));
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
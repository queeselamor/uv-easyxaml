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
    #endregion

    #region Constructors
    public SingleFileConverterViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFile);
        CopyContentCommand = new RelayCommand(CopyContent);
        SaveFileCommand = new AsyncRelayCommand(SaveFile);
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
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IRelayCommand CopyContentCommand { get; }
    public IAsyncRelayCommand SaveFileCommand { get; }
    #endregion

    #region Methods
    private async Task PickFile()
    {
        var filePath = await _fileService.PickFilePath();

        if (filePath != string.Empty)
        {
            CreateConverterItem(filePath);

            await Convert();
        }
    }

    private void CreateConverterItem(string filePath)
    {
        var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath);

        CurrentConverterItem = converterItem;
    }

    private async Task Convert()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        var transformContent = await _imageTransformationManager.Transform(CurrentConverterItem.ConverterType, CurrentConverterItem.SourcePath);
        var resultContent = await _imageTransformationManager.PrepareContent(ConverterType.SvgToXaml, transformContent);

        CurrentConverterItem.ResultContent = resultContent;
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
    }

    private async Task SaveFile()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(CurrentConverterItem.ResultContent))
        {
            return;
        }

        var filePath = await _fileService.SaveFile(CurrentConverterItem.ResultContent, CurrentConverterItem.SourcePath);

        CurrentConverterItem.ResultPath = filePath;
    }
    #endregion

    #region Navigation
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        var filePath = navigationContext.Parameters[NavigationParameterConstants.FilePath] as string;

        CreateConverterItem(filePath);

        Task.Run(Convert);
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
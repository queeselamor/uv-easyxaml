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

    private void CreateConverterItem(string filePath)
    {
        var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath);

        CurrentConverterItem = converterItem;
    }

    private async Task ConvertAsync()
    {
        if (CurrentConverterItem is null)
        {
            return;
        }

        var transformContent = await _imageTransformationManager.TransformAsync(CurrentConverterItem.ConverterType, CurrentConverterItem.SourcePath);
        var resultContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, transformContent);

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

        Task.Run(ConvertAsync);
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
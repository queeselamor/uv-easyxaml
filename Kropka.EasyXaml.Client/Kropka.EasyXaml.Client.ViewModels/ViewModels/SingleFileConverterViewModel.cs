using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class SingleFileConverterViewModel : BaseViewModel, ISingleFileConverterViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IImageTransformationService _imageTransformationService;
    private IConverterItemViewModel _currentConverterItem;
    #endregion

    #region Constructors
    public SingleFileConverterViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFile);
    }

    public SingleFileConverterViewModel(IFileService fileService, IImageTransformationService imageTransformationService) : this()
    {
        _fileService = fileService;
        _imageTransformationService = imageTransformationService;
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
    public IAsyncRelayCommand PickFileCommand { get; set; }
    #endregion

    #region Methods
    private async Task PickFile()
    {
        var filePath = await _fileService.PickFilePath();

        if (filePath != string.Empty)
        {
            var converterItem = new ConverterItemViewModel(ConverterType.SvgToXaml, filePath);

            CurrentConverterItem = converterItem;
        }

        await Convert();
    }

    private async Task Convert()
    {
        var transformContent = await _imageTransformationService.Transform(CurrentConverterItem.ConverterType, CurrentConverterItem.SourcePath);
        var resultContent = await _imageTransformationService.PrepareContent(ConverterType.SvgToXaml, transformContent);

        CurrentConverterItem.ResultContent = resultContent;
    }
    #endregion
}
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class SingleFileConverterViewModel : BaseViewModel, ISingleFileConverterViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IImageTransformationService _imageTransformationService;
    private string _sourceFilePath;
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
    public string SourceFilePath
    {
        get => _sourceFilePath;
        set => SetProperty(ref _sourceFilePath, value);
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
            SourceFilePath = filePath;
        }

        //TODO: Test logic
        var content = await _imageTransformationService.Transform(ConverterType.SvgToXaml, SourceFilePath);
        var preparedContent = await _imageTransformationService.PrepareContent(ConverterType.SvgToXaml, content);
    }
    #endregion
}
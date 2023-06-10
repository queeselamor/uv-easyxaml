using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Model.Base;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels.Model;

public class ConverterItemViewModel : BaseItemViewModel, IConverterItemViewModel
{
    #region Fields
    private ConverterType _converterType;
    private string _sourcePath;
    private string _sourceContent;
    private string _resultPath;
    private string _resultContent;
    private bool _isSelected;
    private string _sourceFileName;
    #endregion

    #region Constructors
    public ConverterItemViewModel(ConverterType converterType, string sourcePath)
    {
        ConverterType = converterType;
        SourcePath = sourcePath;
    } 
    #endregion

    #region Properties
    public ConverterType ConverterType
    {
        get => _converterType;
        set => SetProperty(ref _converterType, value);
    }

    public string SourceFileName
    {
        get => _sourceFileName;
        set => SetProperty(ref _sourceFileName, value);
    }

    public string SourcePath
    {
        get => _sourcePath;
        set => SetProperty(ref _sourcePath, value);
    }

    public string SourceContent
    {
        get => _sourceContent;
        set => SetProperty(ref _sourceContent, value);
    }

    public string ResultPath
    {
        get => _resultPath;
        set => SetProperty(ref _resultPath, value);
    }

    public string ResultContent
    {
        get => _resultContent;
        set => SetProperty(ref _resultContent, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
    #endregion

    #region Methods
    public override string ToString()
    {
        return SourceFileName;
    }
    #endregion
}
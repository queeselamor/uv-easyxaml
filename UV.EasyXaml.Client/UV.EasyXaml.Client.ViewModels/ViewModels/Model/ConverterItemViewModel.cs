using System.Windows.Media;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using UV.EasyXaml.Client.ViewModels.ViewModels.Model.Base;

namespace UV.EasyXaml.Client.ViewModels.ViewModels.Model;

public class ConverterItemViewModel : BaseItemViewModel, IConverterItemViewModel
{
    #region Fields
    private ConverterType _converterType;
    private string _sourcePath;
    private string _sourceContent;
    private string _resultPath;
    private string _resultContent;
    private bool _isSelectedForSave;
    private string _sourceFileName;
    private string _alternativeResultContent;
    private bool _hasTwoContentVariants;
    private string _showingContent;
    private bool _isShowingDrawingContent;
    private Brush _previewBackground;
    #endregion

    #region Constructors
    public ConverterItemViewModel(IConverterItemOwner owner, ConverterType converterType, string sourcePath)
    {
        Owner = owner;
        ConverterType = converterType;
        SourcePath = sourcePath;

        SourceContent = string.Empty;
        ResultContent = string.Empty;
        AlternativeResultContent = string.Empty;
    }
    #endregion

    #region Properties
    public IConverterItemOwner Owner { get; }

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

    public string AlternativeResultContent
    {
        get => _alternativeResultContent;
        set => SetProperty(ref _alternativeResultContent, value);
    }

    public bool IsSelectedForSave
    {
        get => _isSelectedForSave;
        set
        {
            if (!SetProperty(ref _isSelectedForSave, value)) return;

            if (Owner.CanUpdateStates)
            {
                Owner.UpdateStates();
            }
        }
    }

    public bool HasTwoContentVariants
    {
        get => _hasTwoContentVariants;
        set => SetProperty(ref _hasTwoContentVariants, value);
    }

    public string ShowingContent
    {
        get => _showingContent;
        set => SetProperty(ref _showingContent, value);
    }

    public bool IsShowingDrawingContent
    {
        get => _isShowingDrawingContent;
        set => SetProperty(ref _isShowingDrawingContent, value);
    }

    public Brush PreviewBackground
    {
        get => _previewBackground;
        set => SetProperty(ref _previewBackground, value);
    }
    #endregion

    #region Methods
    public override string ToString()
    {
        return SourceFileName;
    }
    #endregion
}
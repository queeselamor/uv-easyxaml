using System.Windows.Media;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Base;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;

public interface IConverterItemViewModel : IViewModel
{
    #region Properties
    public IConverterItemOwner Owner { get; }
    public ConverterType ConverterType { get; set; }
    public string SourceFileName { get; set; }
    public string SourcePath { get; set; }
    public string SourceContent { get; set; }
    public string ResultPath { get; set; }
    public string ResultContent { get; set; }
    public string AlternativeResultContent { get; set; }
    public bool IsSelectedForSave { get; set; }
    public bool HasTwoContentVariants { get; set; }
    public string ShowingContent { get; set; }
    public bool IsShowingDrawingContent { get; set; }
    public Brush PreviewBackground { get; set; }
    #endregion
}
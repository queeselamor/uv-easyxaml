using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Base;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;

public interface IConverterItemViewModel : IViewModel
{
    #region Properties
    public ConverterType ConverterType { get; set; }
    public string SourcePath { get; set; }
    public string SourceContent { get; set; }
    public string ResultPath { get; set; }
    public string ResultContent { get; set; }
    #endregion
}
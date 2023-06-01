using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Base;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;

public interface IConverterItemViewModel : IViewModel
{
    #region Properties
    public ConverterType ConverterType { get; }
    public string SourcePath { get; }
    public string SourceContent { get; }
    public string ResultPath { get; }
    public string ResultContent { get; }
    #endregion
}
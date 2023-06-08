using Kropka.EasyXaml.Client.Infrastructure.Enums;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;

public interface IImageTransformationManager
{
    #region MyRegion
    Task<string> TransformAsync(ConverterType converterType, string filePath);
    Task<string> PrepareContentAsync(ConverterType converterType, string content);
    #endregion
}
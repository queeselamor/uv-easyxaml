using Kropka.EasyXaml.Client.Infrastructure.Enums;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;

public interface IImageTransformationManager
{
    #region MyRegion
    Task<string> Transform(ConverterType converterType, string filePath);
    Task<string> PrepareContent(ConverterType converterType, string content);
    #endregion
}
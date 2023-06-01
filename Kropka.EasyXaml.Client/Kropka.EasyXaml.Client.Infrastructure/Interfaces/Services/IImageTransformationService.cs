using Kropka.EasyXaml.Client.Infrastructure.Enums;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;

public interface IImageTransformationService : IService
{
    #region MyRegion
    Task<string> Transform(ConverterType converterType, string filePath);
    Task<string> PrepareContent(ConverterType converterType, string content);
    #endregion
}
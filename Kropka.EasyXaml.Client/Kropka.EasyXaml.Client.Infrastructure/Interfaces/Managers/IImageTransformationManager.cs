using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;

public interface IImageTransformationManager
{
    #region MyRegion
    Task<bool> CheckTransformationFileExistsAsync(ConverterType converterType);
    Task<IConverterResponse> TransformAsync(ConverterType converterType, string filePath);
    Task<string> PrepareContentAsync(ConverterType converterType, string content);
    #endregion
}
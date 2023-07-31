using System.Windows.Media;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Models;

namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Managers;

public interface IImageTransformationManager
{
    #region Methods
    Task<bool> CheckTransformationFileExistsAsync(ConverterType converterType);
    Task<IConverterResponse> TransformAsync(ConverterType converterType, string filePath);
    Task<string> PrepareContentAsync(ConverterType converterType, string content);
    Brush DetermineBackground(ConverterType converterType, string drawingGroupXaml);
    #endregion
}
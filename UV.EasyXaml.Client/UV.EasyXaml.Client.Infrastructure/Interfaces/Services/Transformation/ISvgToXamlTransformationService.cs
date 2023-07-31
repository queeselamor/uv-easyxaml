using System.Windows.Media;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Models;

namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

public interface ISvgToXamlTransformationService : IService
{
    #region Methods
    Task<bool> CheckTransformationFileExistsAsync();
    Task<IConverterResponse> TransformSvgToXamlAsync(string sourceFile);
    Task<string> PrepareContentAsync(string content);
    Brush DetermineBackground(string drawingGroupXaml);
    #endregion
}
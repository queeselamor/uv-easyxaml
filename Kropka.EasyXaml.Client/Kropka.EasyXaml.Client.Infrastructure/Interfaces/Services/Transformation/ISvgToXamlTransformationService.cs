using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;
using System.Windows.Media;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

public interface ISvgToXamlTransformationService : IService
{
    #region Methods
    Task<bool> CheckTransformationFileExistsAsync();
    Task<IConverterResponse> TransformSvgToXamlAsync(string sourceFile);
    Task<string> PrepareContentAsync(string content);
    Brush DetermineBackground(string drawingGroupXaml);
    #endregion
}
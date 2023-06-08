namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

public interface ISvgToXamlTransformationService : IService
{
    #region Methods
    Task<string> TransformSvgToXamlAsync(string sourceFile);
    Task<string> PrepareContentAsync(string content);
    #endregion
}
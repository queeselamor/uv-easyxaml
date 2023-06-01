namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

public interface ISvgToXamlTransformationService : IService
{
    #region Methods
    Task<string> TransformSvgToXaml(string sourceFile);
    Task<string> PrepareContent(string content);
    #endregion
}
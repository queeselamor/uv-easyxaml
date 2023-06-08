using System;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

namespace Kropka.EasyXaml.Client.Services.Managers;

public class ImageTransformationManager : IImageTransformationManager
{
    #region Fields
    private readonly ISvgToXamlTransformationService _svgToXamlTransformationService;
    #endregion

    #region Constructors
    public ImageTransformationManager(ISvgToXamlTransformationService svgToXamlTransformationService)
    {
        _svgToXamlTransformationService = svgToXamlTransformationService;
    }
    #endregion

    #region Methods
    public Task<string> TransformAsync(ConverterType converterType, string filePath)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => _svgToXamlTransformationService.TransformSvgToXamlAsync(filePath),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }

    public Task<string> PrepareContentAsync(ConverterType converterType, string content)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => _svgToXamlTransformationService.PrepareContentAsync(content),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }
    #endregion
}
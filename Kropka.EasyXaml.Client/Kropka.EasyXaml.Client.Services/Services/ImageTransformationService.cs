using System;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

namespace Kropka.EasyXaml.Client.Services.Services;

public class ImageTransformationService : IImageTransformationService
{
    #region Fields
    private readonly ISvgToXamlTransformationService _svgToXamlTransformationService;
    #endregion

    #region Constructors
    public ImageTransformationService(ISvgToXamlTransformationService svgToXamlTransformationService)
    {
        _svgToXamlTransformationService = svgToXamlTransformationService;
    }
    #endregion

    #region Methods
    public Task<string> Transform(ConverterType converterType, string filePath)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => _svgToXamlTransformationService.TransformSvgToXaml(filePath),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }

    public Task<string> PrepareContent(ConverterType converterType, string content)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => _svgToXamlTransformationService.PrepareContent(content),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }
    #endregion
}
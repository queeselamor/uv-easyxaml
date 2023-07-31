using System;
using System.Threading.Tasks;
using System.Windows.Media;
using UV.EasyXaml.Client.Infrastructure.Constants;
using UV.EasyXaml.Client.Infrastructure.Enums;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Models;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;

namespace UV.EasyXaml.Client.Services.Managers;

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
    public async Task<bool> CheckTransformationFileExistsAsync(ConverterType converterType)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => await _svgToXamlTransformationService.CheckTransformationFileExistsAsync(),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }

    public async Task<IConverterResponse> TransformAsync(ConverterType converterType, string filePath)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => await _svgToXamlTransformationService.TransformSvgToXamlAsync(filePath),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }

    public async Task<string> PrepareContentAsync(ConverterType converterType, string content)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => await _svgToXamlTransformationService.PrepareContentAsync(content),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }

    public Brush DetermineBackground(ConverterType converterType, string drawingGroupXaml)
    {
        return converterType switch
        {
            ConverterType.SvgToXaml => _svgToXamlTransformationService.DetermineBackground(drawingGroupXaml),
            _ => throw new ArgumentOutOfRangeException(nameof(converterType), converterType, MessageConstants.ConverterNotFoundErrorMessage)
        };
    }
    #endregion
}
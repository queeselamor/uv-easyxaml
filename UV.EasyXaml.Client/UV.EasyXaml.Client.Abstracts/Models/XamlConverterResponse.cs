using UV.EasyXaml.Client.Infrastructure.Interfaces.Models;

namespace UV.EasyXaml.Client.Abstracts.Models;

public class XamlConverterResponse : IXamlConverterResponse
{
    #region Properties
    public bool IsSuccessConvertToCanvas { get; set; }
    public string CanvasContent { get; set; } = string.Empty;
    public bool IsSuccessConvertToDrawingGroup { get; set; }
    public string DrawingGroupContent { get; set; } = string.Empty;
    #endregion
}
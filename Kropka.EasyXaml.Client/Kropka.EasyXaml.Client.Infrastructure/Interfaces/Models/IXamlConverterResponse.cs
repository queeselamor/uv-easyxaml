namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;

public interface IXamlConverterResponse : IConverterResponse
{
    #region Properties
    public bool IsSuccessConvertToCanvas { get; }
    public string CanvasContent { get; }
    public bool IsSuccessConvertToDrawingGroup { get; }
    public string DrawingGroupContent { get; }
    #endregion
}
using System.IO;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.Xml;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using Kropka.EasyXaml.Client.Abstracts.Models;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Models;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Linq;

namespace Kropka.EasyXaml.Client.Services.Services.Transformation;

public class SvgToXamlTransformationService : ISvgToXamlTransformationService
{
    #region Constructors
    public SvgToXamlTransformationService()
    {
        AppContext.SetSwitch(ResolverConstants.AllowDefaultResolver, true);
    }
    #endregion

    #region Methods
    public async Task<bool> CheckTransformationFileExistsAsync()
    {
        var isXsltExist = File.Exists(TransformationFilePathConstants.SvgToXamlTransformationFilePath);
        var isColorsExist = File.Exists(TransformationFilePathConstants.SvgToXamlColorsFilePath);

        if (isXsltExist && isColorsExist)
        {
            return await Task.Run(() => true);
        }

        return await Task.Run(() => false);
    }

    public Brush DetermineBackground(string drawingGroupXaml)
    {
        var brushValues = new List<string>();

        var xmlDoc = XDocument.Parse(drawingGroupXaml);

        foreach (var element in xmlDoc.Descendants())
        {
            var brushAttribute = element.Attribute(DetermineBackgroundConstants.BrushProp);
            if (brushAttribute != null)
            {
                brushValues.Add(brushAttribute.Value);
            }

            var colorAttribute = element.Attribute(DetermineBackgroundConstants.ColorProp);
            if (colorAttribute != null)
            {
                brushValues.Add(colorAttribute.Value);
            }

            var fillAttribute = element.Attribute(DetermineBackgroundConstants.FillProp);
            if (fillAttribute != null)
            {
                brushValues.Add(fillAttribute.Value);
            }
        }

        var hasWhite = brushValues.Any(x => x.Contains(DetermineBackgroundConstants.WhiteLong) || 
                                            x.Contains(DetermineBackgroundConstants.WhiteShort) || 
                                            x.ToLower().Contains(DetermineBackgroundConstants.WhiteText));

        if (!hasWhite)
        {
            return Brushes.White;
        }

        var hasBlack = brushValues.Any(x => x.Contains(DetermineBackgroundConstants.BlackLong) || 
                                            x.Contains(DetermineBackgroundConstants.BlackShort) || 
                                            x.ToLower().Contains(DetermineBackgroundConstants.BlackText));

        if (!hasBlack)
        {
            return Brushes.Black;
        }

        var brushConverter = new BrushConverter();

        return brushConverter.ConvertFromString(DetermineBackgroundConstants.Gray) as Brush;
    }

    public async Task<IConverterResponse> TransformSvgToXamlAsync(string sourceFile)
    {
        var response = new XamlConverterResponse();

        var xamlCanvasContent = await GetXamlCanvasContentAsync(sourceFile);

        if (xamlCanvasContent is not null)
        {
            response.IsSuccessConvertToCanvas = true;
            response.CanvasContent = ClearXamlContent(xamlCanvasContent);
        }

        var xamlDrawingGroupContent = await GetXamlDrawingGroupContentAsync(sourceFile);

        if (xamlDrawingGroupContent is not null)
        {
            response.IsSuccessConvertToDrawingGroup = true;
            response.DrawingGroupContent = ClearXamlContent(xamlDrawingGroupContent);
        }

        return await Task.Run(() => response);
    }

    private async Task<string> GetXamlCanvasContentAsync(string sourceFile)
    {
        try
        {
            var svgContent = ClearSvg(sourceFile);

            const string xslTransformFile = TransformationFilePathConstants.SvgToXamlTransformationFilePath;

            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                XmlResolver = new XmlUrlResolver()
            };

            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(xslTransformFile, XsltSettings.TrustedXslt, new XmlUrlResolver());

            using var svgReader = XmlReader.Create(new StringReader(svgContent), settings);
            using var stringWriter = new StringWriter();
            xslTransform.Transform(svgReader, new XsltArgumentList(), stringWriter);
            var xamlContent = stringWriter.ToString();

            return await Task.Run(() => xamlContent);
        }
        catch
        {
            return null;
        }
    }

    private async Task<string> GetXamlDrawingGroupContentAsync(string sourceFile)
    {
        try
        {
            var converter = new FileSvgConverter(new WpfDrawingSettings { IncludeRuntime = false });
            var obj = ConvertSvgFileToWpfObject(sourceFile, converter);
            var xamlContent = ConvertWpfObjectToXaml(obj);

            File.Delete(converter.XamlFile);

            return await Task.Run(() => xamlContent);
        }
        catch
        {
            return null;
        }
    }

    private object ConvertSvgFileToWpfObject(string sourceFile, FileSvgConverter converter)
    {
        converter.Convert(sourceFile);

        return converter.Drawing;
    }

    private string ConvertWpfObjectToXaml(object wpfObject)
    {
        var writer = new XmlXamlWriter(new WpfDrawingSettings { IncludeRuntime = false });
        var xaml = writer.Save(wpfObject);

        return xaml;
    }

    private string ClearSvg(string sourceFile)
    {
        var svgContent = File.ReadAllText(sourceFile);

        svgContent = Regex.Replace(svgContent, @"<!DOCTYPE[^>]+>", "");
        svgContent = Regex.Replace(svgContent, @"<!--[\s\S]*?-->", "");
        svgContent = Regex.Replace(svgContent, @"<metadata>[^<]*<\/metadata>", "");

        return svgContent;
    }

    public async Task<string> PrepareContentAsync(string content)
    {
        char[] charsToTrim = { '\r', '\n' };

        var xamlString = content.TrimStart(charsToTrim).TrimEnd(charsToTrim);

        var lines = xamlString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        for (var i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace("  ", " ");
        }

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0 && lines[i][0] == ' ')
            {
                lines[i] = lines[i].Substring(1);
            }
        }

        return await Task.Run(() => string.Join("\r\n", lines));
    }

    private static string ClearXamlContent(string content)
    {
        RemoveXmlHeader(ref content);
        RemoveSilverlightCompatibility(ref content);
        RemoveXlmnsProperties(ref content);
        RemoveViewboxTags(ref content);
        RemoveCanvasRenderTransform(ref content);
        RemoveCanvasResources(ref content);
        RemoveClipAttribute(ref content);
        RemoveNameAttributes(ref content);
        RemoveEmptyWhitespace(ref content);

        return content;
    }

    private static void RemoveEmptyWhitespace(ref string content)
    {
        var regex = new Regex(@"\s+(?=>)");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveXlmnsProperties(ref string content)
    {
        var regex = new Regex(@"xmlns(:\w+)?=""([^""]+)""");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveXmlHeader(ref string content)
    {
        var regex = new Regex(@"\<\?xml [^>]*>");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveSilverlightCompatibility(ref string content)
    {
        var regex = new Regex(@"\<\!\-\-This file is NOT compatible with Silverlight\-\-\>");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveViewboxTags(ref string content)
    {
        var regex = new Regex(@"<Viewbox[^>]*>|<\/Viewbox>");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveCanvasRenderTransform(ref string content)
    {
        var regex = new Regex(@"<Canvas\.RenderTransform>\s*<TranslateTransform X=""[^""]*"" Y=""[^""]*"" />\s*</Canvas\.RenderTransform>");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveCanvasResources(ref string content)
    {
        var regex = new Regex(@"<Canvas\.Resources>\s*<RectangleGeometry\s+x:Key=""[^""]*""\s+Rect=""[^""]*""\s*/>\s*</Canvas\.Resources>");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveClipAttribute(ref string content)
    {
        var regex = new Regex(@"Clip=""\{StaticResource\s+[^""]*\}""");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveNameAttributes(ref string content)
    {
        var regex = new Regex(@"\s*\bName\b\s*=\s*""[^""]*""");

        RemoveRegexValue(regex, ref content);
    }

    private static void RemoveRegexValue(Regex regex, ref string content)
    {
        var match = regex.Match(content);

        if (match.Success)
        {
            content = regex.Replace(content, "");
        }
    }
    #endregion

}
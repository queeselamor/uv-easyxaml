using System.IO;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.Xml;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace Kropka.EasyXaml.Client.Services.Services.Transformation;

public class SvgToXamlTransformationService : ISvgToXamlTransformationService
{
    #region Methods
    public async Task<string> TransformSvgToXaml(string sourceFile)
    {
        var xamlContent = await GetXamlContent(sourceFile);

        return await Task.FromResult(ClearXamlContent(xamlContent));
    }

    private static Task<string> GetXamlContent(string sourceFile)
    {
        const string xslTransformFile = TransformationFilePathConstants.SvgToXamlTransformationFilePath;

        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse
        };

        var xslTransform = new XslCompiledTransform();
        xslTransform.Load(xslTransformFile);

        using var svgReader = XmlReader.Create(sourceFile, settings);
        using var stringWriter = new StringWriter();
        xslTransform.Transform(svgReader, null, stringWriter);
        var xamlContent = stringWriter.ToString();

        return Task.FromResult(xamlContent);
    }

    public Task<string> PrepareContent(string content)
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

        return Task.FromResult(string.Join("\r\n", lines));
    }

    private static string ClearXamlContent(string content)
    {
        RemoveXmlHeader(ref content);
        RemoveSilverlightCompatibility(ref content);
        RemoveViewboxTags(ref content);
        RemoveCanvasRenderTransform(ref content);
        RemoveCanvasResources(ref content);
        RemoveClipAttribute(ref content);
        RemoveNameAttributes(ref content);

        return content;
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
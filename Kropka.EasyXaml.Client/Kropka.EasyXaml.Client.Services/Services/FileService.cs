using System.IO;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Microsoft.Win32;

namespace Kropka.EasyXaml.Client.Services.Services;

public class FileService : IFileService
{
    #region Methods
    public Task<string> PickFilePathAsync()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = ExtensionFilterConstants.SvgFilter
        };

        var result = openFileDialog.ShowDialog();

        return Task.FromResult(result == true ? openFileDialog.FileName : string.Empty);
    }

    public async Task<string> SaveFileAsync(string content, string sourceFilePath)
    {
        var fileName = string.Empty;
        if (!string.IsNullOrEmpty(sourceFilePath))
        {
            fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = ExtensionFilterConstants.XamlFilter,
            FileName = fileName
        };

        if (saveFileDialog.ShowDialog() != true) return string.Empty;

        var filePath = saveFileDialog.FileName;

        await File.WriteAllTextAsync(filePath, content);

        return filePath;
    }
    #endregion
}
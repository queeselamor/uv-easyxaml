using System.IO;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Microsoft.Win32;

namespace Kropka.EasyXaml.Client.Services.Services;

public class FileService : IFileService
{
    #region Methods
    public Task<string> PickFilePath()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "SVG Files (*.svg)|*.svg"
        };

        var result = openFileDialog.ShowDialog();

        return Task.FromResult(result == true ? openFileDialog.FileName : string.Empty);
    }

    public async Task<string> SaveFile(string content, string sourceFilePath)
    {
        var fileName = string.Empty;
        if (!string.IsNullOrEmpty(sourceFilePath))
        {
            fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "XAML files (*.xaml)|*.xaml",
            FileName = fileName
        };

        if (saveFileDialog.ShowDialog() != true) return string.Empty;

        var filePath = saveFileDialog.FileName;

        await File.WriteAllTextAsync(filePath, content);

        return filePath;
    }
    #endregion

}
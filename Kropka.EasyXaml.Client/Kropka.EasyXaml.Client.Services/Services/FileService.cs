using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

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

    public async Task<string> SaveFileAsync(string content, string sourceFilePath, string folderPath)
    {
        var fileName = string.Empty;
        if (!string.IsNullOrEmpty(sourceFilePath))
        {
            fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        }

        var savePath = Path.Combine(folderPath, $"{fileName}.xaml");

        await File.WriteAllTextAsync(savePath, content);

        return savePath;
    }

    public Task<string> PickFolderAsync()
    {
        var folderDialog = new FolderBrowserDialog();

        var result = folderDialog.ShowDialog();

        return Task.FromResult(result == DialogResult.OK ? folderDialog.SelectedPath : string.Empty);
    }

    public Task<IEnumerable<string>> GetFilePathsAsync(string folderPath)
    {
        var filePaths = Directory.GetFiles(folderPath).Where(path => path.EndsWith(ExtensionFilterConstants.SvgExtensionFilter));

        return Task.FromResult(filePaths.AsEnumerable());
    }
    #endregion
}
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
    #endregion

}
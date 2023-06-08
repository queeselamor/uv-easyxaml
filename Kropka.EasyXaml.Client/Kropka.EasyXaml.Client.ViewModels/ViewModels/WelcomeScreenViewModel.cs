using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using System.Threading.Tasks;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class WelcomeScreenViewModel : BaseViewModel, IWelcomeScreenViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    #endregion

    #region Constructors
    public WelcomeScreenViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFile);
    }

    public WelcomeScreenViewModel(IFileService fileService) : this()
    {
        _fileService = fileService;
    }
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    #endregion

    #region Methods
    private async Task PickFile()
    {
        var filePath = await _fileService.PickFilePath();

        if (filePath != string.Empty)
        {
            
        }
    }
    #endregion
}
using CommunityToolkit.Mvvm.Input;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Regions;
using Kropka.EasyXaml.Client.Infrastructure.Managers;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class WelcomeScreenViewModel : BaseViewModel, IWelcomeScreenViewModel
{
    #region Fields
    private readonly IFileService _fileService;
    private readonly IRegionManager _regionManager;
    #endregion

    #region Constructors
    public WelcomeScreenViewModel()
    {
        PickFileCommand = new AsyncRelayCommand(PickFileAsync);
        PickFolderCommand = new AsyncRelayCommand(PickFolderAsync);
    }

    public WelcomeScreenViewModel(IFileService fileService, IRegionManager regionManager) : this()
    {
        _fileService = fileService;
        _regionManager = regionManager;
    }
    #endregion

    #region Commands
    public IAsyncRelayCommand PickFileCommand { get; }
    public IAsyncRelayCommand PickFolderCommand { get; }
    #endregion

    #region Methods
    private async Task PickFileAsync()
    {
        var filePath = await _fileService.PickFilePathAsync();

        if (filePath != string.Empty)
        {
            _regionManager.RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IConverterView));

            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FilePath, filePath
                }
            };

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView, parameters);
        }
    }

    private async Task PickFolderAsync()
    {
        var folderPath = await _fileService.PickFolderAsync();

        if (folderPath != string.Empty)
        {
            _regionManager.RegisterViewWithRegion(RegionNameConstants.MainRegion, typeof(IConverterView));

            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FolderPath, folderPath
                }
            };

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.MainRegion, ViewNameConstants.ConverterView, parameters);
        }
    }
    #endregion
}
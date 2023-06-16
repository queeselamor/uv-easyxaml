using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Extensions;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Kropka.EasyXaml.Client.Infrastructure.Managers;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class ConverterViewModel : BaseViewModel, IConverterViewModel
{
    #region Fields
    private readonly IRegionManager _regionManager;
    private int _selectedTabIndex;
    #endregion

    #region Constructors
    public ConverterViewModel()
    {
        Initialize();
    }

    public ConverterViewModel(IRegionManager regionManager) :this()
    {
        _regionManager = regionManager;
    }
    #endregion

    #region Properties
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    #endregion

    #region Methods
    public static void Initialize()
    {
        ViewExtensions.GetView<ISingleFileConverterView>().ActivateRegionWithView(RegionNameConstants.SingleFileConverterRegion);
        ViewExtensions.GetView<IFolderConverterView>().ActivateRegionWithView(RegionNameConstants.FolderConverterRegion);
    }
    #endregion

    #region Navigation
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        if (navigationContext.Parameters[NavigationParameterConstants.FilePath] is string filePath)
        {
            SelectedTabIndex = 0;

            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FilePath, filePath
                }
            };

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.SingleFileConverterRegion, ViewNameConstants.SingleFileConverterView, parameters);
        }

        if (navigationContext.Parameters[NavigationParameterConstants.FolderPath] is string folderPath)
        {
            var parameters = new NavigationParameters
            {
                {
                    NavigationParameterConstants.FolderPath, folderPath
                }
            };

            SelectedTabIndex = 1;

            RegionNavigationManager.Navigate(_regionManager, RegionNameConstants.FolderConverterRegion, ViewNameConstants.FolderConverterView, parameters);
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
    #endregion
}
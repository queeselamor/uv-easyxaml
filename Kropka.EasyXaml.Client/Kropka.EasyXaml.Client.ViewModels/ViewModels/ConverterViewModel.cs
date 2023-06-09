using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Extensions;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class ConverterViewModel : BaseViewModel, IConverterViewModel
{
    #region Constructors
    public ConverterViewModel()
    {
        Initialize();
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
            //logic
        }

        if (navigationContext.Parameters[NavigationParameterConstants.FolderPath] is string folderPath)
        {
            //logic
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
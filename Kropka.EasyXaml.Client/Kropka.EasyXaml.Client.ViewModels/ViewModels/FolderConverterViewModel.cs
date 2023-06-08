using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kropka.EasyXaml.Client.Infrastructure.Constants;
using Kropka.EasyXaml.Client.Infrastructure.Enums;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Model;
using Kropka.EasyXaml.Client.ViewModels.ViewModels.Base;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.ViewModels.ViewModels;

public class FolderConverterViewModel : BaseViewModel, IFolderConverterViewModel
{
    #region Fields
    private ObservableCollection<IConverterItemViewModel> _converterItems;
    #endregion

    #region Constructors
    public FolderConverterViewModel()
    {
        ConverterItems = new ObservableCollection<IConverterItemViewModel>();
    }
    #endregion

    #region Properties
    public ObservableCollection<IConverterItemViewModel> ConverterItems
    {
        get => _converterItems;
        set => SetProperty(ref _converterItems, value);
    }
    #endregion

    #region Methods
    private async Task ConvertItemsAsync()
    {
        if (ConverterItems is null)
        {
            return;
        }

        foreach (var converterItemViewModel in ConverterItems)
        {
            //var transformContent = await _imageTransformationManager.TransformAsync(converterItemViewModel.ConverterType, converterItemViewModel.SourcePath);
            //var resultContent = await _imageTransformationManager.PrepareContentAsync(ConverterType.SvgToXaml, transformContent);

            //converterItemViewModel.ResultContent = resultContent;
        }
    }
    #endregion

    #region Navigation
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.Count == 0)
        {
            return;
        }

        var folderPath = navigationContext.Parameters[NavigationParameterConstants.FolderPath] as string;
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
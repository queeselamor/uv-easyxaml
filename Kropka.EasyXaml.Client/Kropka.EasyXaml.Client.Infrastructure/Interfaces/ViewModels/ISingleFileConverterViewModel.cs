using Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels.Base;
using Prism.Regions;

namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;

public interface ISingleFileConverterViewModel : IViewModel, INavigationAware, IConverterItemOwner
{
}
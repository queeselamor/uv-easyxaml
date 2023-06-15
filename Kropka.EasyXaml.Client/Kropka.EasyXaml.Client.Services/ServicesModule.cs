using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;
using Kropka.EasyXaml.Client.Services.Managers;
using Kropka.EasyXaml.Client.Services.Services;
using Kropka.EasyXaml.Client.Services.Services.Transformation;
using Prism.Ioc;
using Prism.Modularity;

namespace Kropka.EasyXaml.Client.Services;

public class ServicesModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IFileService, FileService>();
        containerRegistry.RegisterSingleton<ISvgToXamlTransformationService, SvgToXamlTransformationService>();
        containerRegistry.RegisterSingleton<IImageTransformationManager, ImageTransformationManager>();
        containerRegistry.RegisterSingleton<IBusyService, BusyService>();
    }
}
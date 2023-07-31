using Prism.Ioc;
using Prism.Modularity;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Managers;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Services.Transformation;
using UV.EasyXaml.Client.Services.Managers;
using UV.EasyXaml.Client.Services.Services;
using UV.EasyXaml.Client.Services.Services.Transformation;

namespace UV.EasyXaml.Client.Services;

public class ServicesModule : IModule
{
    #region Methods
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
    #endregion
}
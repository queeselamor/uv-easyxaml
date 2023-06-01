using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;
using Kropka.EasyXaml.Client.Services.Services;
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
    }
}
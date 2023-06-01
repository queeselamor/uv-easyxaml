namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;

public interface IFileService : IService
{
    #region Methods
    Task<string> PickFilePath();
    #endregion
}
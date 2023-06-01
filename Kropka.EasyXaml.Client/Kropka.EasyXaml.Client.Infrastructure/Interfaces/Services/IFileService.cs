namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.Services;

public interface IFileService : IService
{
    #region Methods
    Task<string> PickFilePath();
    Task<string> SaveFile(string content, string sourceFilePath);
    #endregion
}
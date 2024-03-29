﻿namespace UV.EasyXaml.Client.Infrastructure.Interfaces.Services;

public interface IFileService : IService
{
    #region Methods
    Task<string> PickFilePathAsync();
    Task<string> SaveFileAsync(string content, string sourceFilePath);
    Task<string> SaveFileAsync(string content, string sourceFilePath, string folderPath);
    Task<string> PickFolderAsync();
    Task<IEnumerable<string>> GetFilePathsAsync(string folderPath);
    Task<string> GetFileNameAsync(string folderPath);
    Task<bool> CheckFileExtension(string filePath, string fileExtension);
    Task<bool> CheckIsFolder(string folderPath);
    #endregion
}
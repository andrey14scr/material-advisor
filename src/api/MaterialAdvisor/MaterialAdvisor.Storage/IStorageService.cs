using Microsoft.AspNetCore.Http;

namespace MaterialAdvisor.Storage;

public interface IStorageService
{
    Task<string> SaveFile(IFormFile file);

    Task<string> SaveFile(Stream stream, string fileName);

    Task<FileToDownload> GetFile(string name);

    Task DeleteFile(string name);
}
using Microsoft.AspNetCore.Http;

namespace MaterialAdvisor.Application.Storage;

public interface IStorageService
{
    Task<string> SaveFile(IFormFile file);

    Task<FileToDownload> GetFile(string name);

    Task DeleteFile(string name);
}
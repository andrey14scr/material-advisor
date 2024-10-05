using Microsoft.AspNetCore.Http;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string name);

    Task<byte[]> GetFileAsync(string name);

    Task DeleteFile(string name);
}
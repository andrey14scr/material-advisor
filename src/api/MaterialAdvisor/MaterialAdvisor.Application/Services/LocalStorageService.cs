using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaterialAdvisor.Application.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _storageDirectory;

    public LocalStorageService(IOptions<StorageOptions> storageOptions)
    {
        _storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), storageOptions.Value.Root);
    }

    public async Task<string> SaveFileAsync(IFormFile file, string name)
    {
        var filePath = Path.Combine(_storageDirectory, name);

        if (!Directory.Exists(_storageDirectory))
        {
            Directory.CreateDirectory(_storageDirectory);
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }

    public async Task<byte[]> GetFileAsync(string name)
    {
        var filePath = Path.Combine(_storageDirectory, name);

        if (!File.Exists(filePath))
        {
            throw new ArgumentException($"File {filePath} was not found", nameof(name));
        }

        return await File.ReadAllBytesAsync(filePath);
    }

    public Task DeleteFile(string name)
    {
        var filePath = Path.Combine(_storageDirectory, name);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}

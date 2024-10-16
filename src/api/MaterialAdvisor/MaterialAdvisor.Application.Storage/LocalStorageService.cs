using MaterialAdvisor.Application.Storage.Configuration.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using System.IO;

namespace MaterialAdvisor.Application.Storage;

public class LocalStorageService : AbstractStorageService, IStorageService
{
    private readonly string _storageDirectory;

    public LocalStorageService(IOptions<StorageOptions> storageOptions)
    {
        _storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), storageOptions.Value.Root);
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        var filePath = Path.Combine(_storageDirectory, GetUniqueFileName(file.FileName));

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

    public async Task<FileToDownload> GetFile(string name)
    {
        var filePath = Path.Combine(_storageDirectory, name);

        if (!File.Exists(filePath))
        {
            throw new ArgumentException($"File {filePath} was not found", nameof(name));
        }

        return new FileToDownload 
        { 
            Data = await File.ReadAllBytesAsync(filePath), 
            OriginalName = GetOriginalFileName(name) 
        };
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

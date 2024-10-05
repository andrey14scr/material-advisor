using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaterialAdvisor.Application.Services;

public class BlobStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(IOptions<AzureOptions> azureOptions, IOptions<StorageOptions> storageOptions)
    {
        _containerName = storageOptions.Value.Root;
        _blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);
    }

    public async Task<string> SaveFileAsync(IFormFile file, string name)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(_containerName);
        var blobClient = blobContainerClient.GetBlobClient($"{name}_{Guid.NewGuid()}");

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();
    }

    public async Task<byte[]> GetFileAsync(string name)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(name);

        if (!await blobClient.ExistsAsync())
        {
            throw new NotFoundException();
        }

        var downloadResponse = await blobClient.DownloadAsync();
        using (var memoryStream = new MemoryStream())
        {
            await downloadResponse.Value.Content.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public async Task DeleteFile(string name)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(name);

        await blobClient.DeleteIfExistsAsync();
    }

    private async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        return blobContainerClient;
    }
}
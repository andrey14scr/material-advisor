using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using MaterialAdvisor.Application.Storage.Configuration.Options;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaterialAdvisor.Application.Storage;

public class BlobStorageService : AbstractStorageService, IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(IOptions<AzureOptions> azureOptions, IOptions<StorageOptions> storageOptions)
    {
        _containerName = storageOptions.Value.Root;
        _blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(GetUniqueFileName(file.FileName));

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);
        

        return blobClient.Name;
    }

    public async Task<FileToDownload> GetFile(string name)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(name);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException();
        }

        using var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        return new FileToDownload
        {
            Data = memoryStream.ToArray(),
            OriginalName = GetOriginalFileName(name)
        };
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
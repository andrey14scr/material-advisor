using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Application.TableGenerator;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.SignalR.Hubs;
using MaterialAdvisor.Storage;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.Application.QueueStorage.Handlers;

public class GenerateTablesMessageHandler(IHubContext<TableGenerationHub> _tableGenerationHubContext,
    ILogger<GenerateTablesMessageHandler> _logger,
    ITableGenerator _tableGenerator,
    IStorageService _storageService,
    MaterialAdvisorContext _dbContext) : IMessageHandler<GenerateTablesMessage>
{
    public async Task HandleAsync(GenerateTablesMessage message)
    {
        try
        {
            _logger.LogDebug($"Handled {nameof(GenerateTablesMessage)}");

            var user = await _dbContext.Users.SingleAsync(u => u.Name == message.UserName);

            var files = await _tableGenerator.GenerateTable(message.TableGenerationParameters);
            await SaveFiles(files, user);

            await _tableGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TableGeneratedMessage, message.TableGenerationParameters.Select(p => p.KnowledgeCheckId), GenerationStatuses.Generated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing handler");

            await _tableGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TableGeneratedMessage, message.TableGenerationParameters.Select(p => p.KnowledgeCheckId), GenerationStatuses.Failed);

            throw;
        }
    }

    private async Task SaveFiles(IEnumerable<TableGenerationResponse> files, UserEntity user)
    {
        var now = DateTime.UtcNow;
        foreach (var file in files)
        {
            if (file.File is not null)
            {
                await _dbContext.GeneratedFiles
                    .Where(gf => gf.Id == file.PreGeneratedFileId)
                    .ExecuteUpdateAsync(gf => gf.SetProperty(p => p.File, file.File)
                        .SetProperty(p => p.GeneratedAt, now));
            }
            else
            {
                await _dbContext.GeneratedFilesKnowldgeChecks
                    .Where(gfkc => gfkc.GeneratedFileId == file.PreGeneratedFileId)
                    .ExecuteDeleteAsync();
                await _dbContext.GeneratedFiles
                    .Where(gf => gf.Id == file.PreGeneratedFileId)
                    .ExecuteDeleteAsync();
            }
        }

        await ClearOldFiles(files);
    }

    private async Task ClearOldFiles(IEnumerable<TableGenerationResponse> files)
    {
        foreach (var file in files)
        {
            var existing = await _dbContext.GeneratedFiles
                .Where(gf => gf.File != null &&
                    gf.GeneratedFilesKnowldgeChecks.Any(gfkc => gfkc.KnowledgeCheckId == file.KnowledgeCheckId))
                .OrderByDescending(gf => gf.GeneratedAt)
                .ToListAsync();

            var skip = 1;
            if (existing.Count > skip)
            {
                var toDelete = existing.Skip(skip).ToList();

                foreach (var item in toDelete)
                {
                    await _storageService.DeleteFile(item.File!);

                    await _dbContext.GeneratedFilesKnowldgeChecks
                        .Where(gfkc => gfkc.GeneratedFileId == item.Id)
                        .ExecuteDeleteAsync();
                    await _dbContext.GeneratedFiles
                        .Where(gf => gf.Id == item.Id)
                        .ExecuteDeleteAsync();
                }
            }
        }
    }
}
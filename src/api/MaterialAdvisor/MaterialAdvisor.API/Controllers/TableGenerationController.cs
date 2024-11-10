using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Application.QueueStorage.QueueService;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Application.TableGenerator;
using MaterialAdvisor.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class TableGenerationController(IStorageService _storageService, 
    IUserProvider _userService,
    IKnowledgeCheckService _knowledgeCheckService,
    IMessagesQueueService _messageQueueService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> Receive(string file)
    {
        var fileToDownload = await _storageService.GetFile(file);
        return File(fileToDownload.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileToDownload.OriginalName);
    }

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody]Guid knowledgeCheckId)
    {
        var hasVerifiedAttemts = await _knowledgeCheckService.HasVerifiedAttemts(knowledgeCheckId);
        if (!hasVerifiedAttemts)
        {
            return BadRequest(knowledgeCheckId);
        }

        var preGeneratedFileId = await _knowledgeCheckService.AddPreGeneratedFile(knowledgeCheckId);

        var user = await _userService.GetUser();

        var message = new GenerateTablesMessage
        {
            UserName = user.Name,
            TableGenerationParameters = 
            [
                new TableGenerationParameter 
                {
                    KnowledgeCheckId = knowledgeCheckId,
                    PreGeneratedFileId = preGeneratedFileId,
                }
            ]
        };
        _messageQueueService.SendMessage(message);

        return Ok(knowledgeCheckId);
    }
}
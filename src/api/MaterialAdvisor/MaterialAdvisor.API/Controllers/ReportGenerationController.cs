using MaterialAdvisor.API.Models.Requests.ReportGeneration;
using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Application.TableGenerator;
using MaterialAdvisor.QueueStorage;
using MaterialAdvisor.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class ReportGenerationController(IStorageService _storageService, 
    IUserProvider _userService,
    IKnowledgeCheckService _knowledgeCheckService,
    IGeneratedFileService _generatedFileService,
    IMessagesQueueService _messageQueueService) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] ReportGenerationRequest reportGenerationRequest)
    {
        var hasVerifiedAttemts = await _knowledgeCheckService.HasVerifiedAttemts(reportGenerationRequest.KnowledgeCheckId);
        if (!hasVerifiedAttemts)
        {
            return BadRequest(reportGenerationRequest.KnowledgeCheckId);
        }

        var preGeneratedFile = await _generatedFileService.AddPreGeneratedFile<GeneratedFile>(reportGenerationRequest.KnowledgeCheckId);

        var user = await _userService.GetUser();

        var message = new GenerateTablesMessage
        {
            UserName = user.Name,
            TableGenerationParameters = 
            [
                new TableGenerationParameter 
                {
                    KnowledgeCheckId = reportGenerationRequest.KnowledgeCheckId,
                    PreGeneratedFileId = preGeneratedFile.Id,
                }
            ]
        };
        _messageQueueService.SendMessage(message);

        return Ok(preGeneratedFile);
    }
}
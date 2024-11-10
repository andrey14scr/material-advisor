using MaterialAdvisor.API.Models.Requests.TopicGeneration;
using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Application.QueueStorage.QueueService;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class TopicGenerationController(ITopicService _topicService,
    IStorageService _storageService,
    IUserProvider _userService, 
    IMessagesQueueService _messageQueueService) : BaseApiController
{ 
    [HttpPost()]
    public async Task<ActionResult> Generate([FromForm] ReportGenerationRequest request)
    {
        var file = await _storageService.SaveFile(request.File);
        var topicToCreate = new Topic
        {
            Name = request.TopicName,
            File = file,
            Version = 0,
            GeneratedAt = DateTime.UtcNow,
        };
        var createdTopic = await _topicService.Create(topicToCreate);

        var user = await _userService.GetUser();
        var metadata = JsonSerializer.Serialize(HttpContext.Items);

        var message = new GenerateTopicMessage
        {
            TopicId = createdTopic.Id,
            UserName = user.Name,
            DoesComplexityIncrease = request.DoesComplexityIncrease,
            CultureContext = request.CultureContext,
            DefaultAnswersCount = request.DefaultAnswersCount,
            Languages = request.Languages,
            QuestionsStructure = request.QuestionsStructure,
            Metadata = metadata,
        };
        _messageQueueService.SendMessage(message);

        return Ok(createdTopic);
    }
}

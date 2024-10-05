using MaterialAdvisor.API.Models.Requests.TopicGeneration;
using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.QueueStorage.Messages;
using MaterialAdvisor.QueueStorage.QueueService;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

//[Authorize]
public class TopicGenerationController(ITopicService _topicService,
    IStorageService _storageService, 
    IUserProvider _userService, 
    IMessagesQueueService _messageQueueService) : BaseApiController
{ 
    [HttpPost()]
    public async Task<ActionResult> Generate([FromForm]TopicGenerationRequest request)
    {
        var filePath = await _storageService.SaveFileAsync(request.File, request.File.FileName);
        var topicToCreate = new Topic()
        {
            Name = request.TopicName,
            FilePath = filePath,
            Version = 0,
        };
        var createdTopic = await _topicService.Create(topicToCreate);

        var user = await _userService.GetUser();
        var message = new GenerateTopicMessage()
        {
            TopicId = createdTopic.Id,
            UserName = user.UserName,
        };
        _messageQueueService.SendMessage(message);

        return Ok(createdTopic);
    }
}

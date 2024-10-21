﻿using Azure.AI.OpenAI;
using Azure;
using OpenAI.Assistants;
using OpenAI.Files;
using MaterialAdvisor.Application.Storage;
using Microsoft.Extensions.Options;
using MaterialAdvisor.Application.AI.Configuration.Options;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.Application.AI;

#pragma warning disable OPENAI001

public class MaterialAdvisorAIAssistant : IMaterialAdvisorAIAssistant
{
    private readonly IStorageService storageService;
    private readonly AzureOpenAIClient _openAIClient;
    private readonly OpenAIAssistantOptions _openAIAssistantOptions;
    private readonly ILogger<MaterialAdvisorAIAssistant> _logger;

    public MaterialAdvisorAIAssistant(IStorageService _storageService, 
        IOptions<OpenAIAssistantOptions> openAIAssistantOptions, 
        ILogger<MaterialAdvisorAIAssistant> logger)
    {
        storageService = _storageService;

        var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
        var key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")!;

        _openAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
        _openAIAssistantOptions = openAIAssistantOptions.Value;
        _logger = logger;
    }

    public async Task<string> GenerateQuestions(string filename, string prompt)
    {
        var responseJson = "{}";

        _logger.LogInformation("[AI Assistant]: Get assistant client...");
        var assistantClient = _openAIClient.GetAssistantClient();
        var assistant = (await assistantClient.GetAssistantAsync(_openAIAssistantOptions.AssistantId)).Value;
        _logger.LogInformation($"[AI Assistant]: Assistent {assistant.Id} got");

        _logger.LogInformation("[AI Assistant]: Upload attachment file...");
        var vectorizedAttachment = await GetOpenAIFile(filename);
        _logger.LogInformation($"[AI Assistant]: Attachment file {vectorizedAttachment.Id} ({vectorizedAttachment.Filename}) uploaded");

        _logger.LogInformation("[AI Assistant]: Create thread...");
        var thread = (await assistantClient.CreateThreadAsync()).Value;
        _logger.LogInformation($"[AI Assistant]: Thread {thread.Id} created");

        var messageCreationOptions = new MessageCreationOptions
        { 
            Attachments = [new MessageCreationAttachment(vectorizedAttachment.Id, [ToolDefinition.CreateFileSearch()])]
        };

        var content = CreateContent(prompt);
        _logger.LogInformation("[AI Assistant]: Send message...");
        _logger.LogInformation($"[AI Assistant]: Message Content: {content}");
        await assistantClient.CreateMessageAsync(thread.Id, MessageRole.User, content, messageCreationOptions);

        await foreach (var streamingUpdate in assistantClient.CreateRunStreamingAsync(thread.Id, assistant.Id, new RunCreationOptions()))
        {
            if (streamingUpdate.UpdateKind == StreamingUpdateReason.RunCreated)
            {
                _logger.LogInformation("[AI Assistant]: Streaming - run created");
            }
            else if (streamingUpdate is MessageContentUpdate contentUpdate)
            {
                if (contentUpdate?.TextAnnotation?.InputFileId == vectorizedAttachment.Id)
                {
                    _logger.LogInformation($"[AI Assistant]: Streaming - attachment added: {vectorizedAttachment.Id} ({vectorizedAttachment.Filename})");
                }
                else
                {
                    _logger.LogInformation($"[AI Assistant]: Streaming - response received: {contentUpdate?.Text})");
                    if (contentUpdate?.Text is not null)
                    {
                        responseJson = contentUpdate.Text;
                    }
                }
            }
        }

        _logger.LogInformation("[AI Assistant]: Clear attachment...");
        await _openAIClient.GetOpenAIFileClient().DeleteFileAsync(vectorizedAttachment.Id);
        _logger.LogInformation($"[AI Assistant]: Attachment {vectorizedAttachment.Id} deleted");

        _logger.LogInformation("[AI Assistant]: Clear thread...");
        await assistantClient.DeleteThreadAsync(thread.Id);
        _logger.LogInformation($"[AI Assistant]: Thread {thread.Id} deleted");

        return responseJson;
    }

    private async Task<OpenAIFile> GetOpenAIFile(string filename)
    {
        var fileFromStorage = await storageService.GetFile(filename);
        var fileStream = new MemoryStream(fileFromStorage.Data);
        var fileClient = _openAIClient.GetOpenAIFileClient();
        var vectorizedFile = await fileClient.UploadFileAsync(fileStream, fileFromStorage.OriginalName, FileUploadPurpose.Assistants);
        return vectorizedFile.Value;
    }

    private IEnumerable<MessageContent> CreateContent(string prompt)
    {
        return [MessageContent.FromText(prompt)];
    }
}

#pragma warning restore OPENAI001